using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetDexTest_01.Models;
using NetDexTest_01.Models.Entities;

using NetDexTest_01.Services;
using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text;


namespace NetDexTest_01.Controllers
{


    // this says that "You must be Authenticated to access the endpoints"
    // can be placed at individual endpoints, as well, but...
    // having this at the controller-level means that all endpoints in controller require authorization
    // (basically: globally applied)
    // [Authorize]
    public partial class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Random _random = new Random();



        public HomeController(IUserRepository userRepo, UserManager<ApplicationUser> userManager, ILogger<HomeController> logger)
        {
            _logger = logger;
            _userRepo = userRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }



        //public async Task<IActionResult> IndexUser()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null) return NotFound();

        //    var result = await _userRepo.GetByUsernameAsync(user.UserName);
        //    return View(result);

        //}


        // CHATGPT
        public async Task<IActionResult> Gptindex()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var dexHolder = await _userRepo.GetDexHolderByUserIdAsync(user.Id);
            if (dexHolder == null)
            {
                dexHolder = new DexHolder
                {
                    ApplicationUserId = user.Id,
                    ApplicationUserName = user.UserName,
                    FirstName = "First",
                    MiddleName = "Middle",
                    LastName = "Last",
                    Gender = "Female",
                    Pronouns = "She/Her"
                };

                await _userRepo.AddDexHolderAsync(dexHolder);
                await _userRepo.SaveChangesAsync();
            }
            return View(dexHolder);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }






        [AllowAnonymous]        // YOU DONT NEED TO BE AUTHENTICATED TO ACCESS THIS ENDPOINT
        public IActionResult About()
        {
            ViewData["About"] = "My Excellent App!";
            return View();
        }



        #region authorization

        [Authorize]
        public IActionResult Restricted()
        {
            return Content("This is restricted.");
        }


        [Authorize(Roles = "HRManager,Finance")]
        public IActionResult RestrictedRoles()
        {
            return Content("This is restricted.");
        }


        #endregion authorization











    }
}
