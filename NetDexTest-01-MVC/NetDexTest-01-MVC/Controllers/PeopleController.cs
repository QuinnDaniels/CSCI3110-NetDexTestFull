using Microsoft.AspNetCore.Mvc;
using System.Net;
using NetDexTest_01_MVC.Models;
using NetDexTest_01_MVC.Models.ViewModels;
using NetDexTest_01_MVC.Models.Entities;
using NetDexTest_01_MVC.Models.Authentication;
using NetDexTest_01_MVC.Services;

namespace NetDexTest_01_MVC.Controllers
{
    [Route("dex")]
    public class PeopleController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private IAuthService _authService;
        private readonly IUserSessionService _userSessionService;
        private readonly IUserService _userService;
        private readonly IPersonService _personService;


        public PeopleController(ILogger<AuthController> logger,
            IAuthService authService,
            IUserService userService,
            IPersonService personService,
            IUserSessionService userSessionService)
        {
            _logger = logger;
            _authService = authService;
            _userSessionService = userSessionService;
            _userService = userService;
            _personService = personService;

        }





        //public IActionResult Index()
        //{
        //    return View();
        //}


        [HttpGet("", Name = "DexIndex")]
        public async Task<IActionResult> ListDexIndex()
        {
            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");
            
            if (await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator", "User"))
            {

                //var req = _userSessionService.GetEmail();
                //var dex = await _personService.GetDexHolderMiddleVMAsync(req);

                ViewData["LoggedInEmail"] = _userSessionService.GetEmail();

                ////ICollection<AdminUserVM> userList = await _userService.GetAllUsersAdminAsync();
                ////if(userList != null )
                ////{
                //return View(dex);// userList);
                //}
                //return BadRequest();

                return View();
            }
            return Unauthorized();

            //return View();
        }



        [HttpGet("{id}", Name = "DexIndexSingle")]
        public async Task<IActionResult> ListDexIndex(string id)
        {

            if (await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator"))
            {

                //var req = _userSessionService.GetEmail();
                //var dex = await _personService.GetDexHolderMiddleVMAsync(req);

                ViewData["LoggedInEmail"] = _userSessionService.GetEmail();

                ////ICollection<AdminUserVM> userList = await _userService.GetAllUsersAdminAsync();
                ////if(userList != null )
                ////{
                //return View(dex);// userList);
                //}
                //return BadRequest();

                return View();
            }
            else 
            {
                // TODO : check if the input matches the logged in user
            }
            return Unauthorized();

            //return View();
        }

        [HttpGet("create")]
        public async Task<IActionResult> CreatePerson()
        {
            return View();
        }

        [HttpPost("create"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePerson(NewPersonVM personVM)
        {
            if (!ModelState.IsValid)
            {
                //return error messages
                return View(personVM);
            }

            var response = await _personService.CreatePersonAsync(personVM);
            if (response.Status == HttpStatusCode.Unauthorized)
            {
                //if response is 401, it means access token has expired
                return RedirectToAction("refresh", "auth", new { returnUrl = "/people/createPerson" });
            }
            if (response.Status != HttpStatusCode.OK)
            {
                ModelState.AddModelError(string.Empty, "An Error Occurred while processing this request. Please try again in some time.");
            }
            return Ok("Person Created");
        }




    }
}
