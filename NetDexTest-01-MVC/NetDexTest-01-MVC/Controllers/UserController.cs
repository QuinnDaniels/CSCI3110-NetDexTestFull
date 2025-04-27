using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetDexTest_01_MVC.Models.ViewModels;
using NetDexTest_01_MVC.Services;

namespace NetDexTest_01_MVC.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {

        private readonly ILogger<AuthController> _logger;
        private IAuthService _authService;
        private readonly IUserSessionService _userSessionService;
        private readonly IUserService _userService;


        public UserController(ILogger<AuthController> logger,
            IAuthService authService,
            IUserService userService,
            IUserSessionService userSessionService)
        {
            _logger = logger;
            _authService = authService;
            _userSessionService = userSessionService;
            _userService = userService;

        }


        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }


        [Authorize(Roles = "Administrator")]
        public IActionResult TestRoleCheck()
        {
            return Content("Restricted to role TestRole");
        }

        [HttpGet("admin/all")]
        //[Authorize(Roles = "Administrator")]
        //[Route("")]
        public async Task<IActionResult> ListUsers()
        {
            if (await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator"))
            {
                //ICollection<AdminUserVM> userList = await _userService.GetAllUsersAdminAsync();
                //if(userList != null )
                //{
                return View();// userList);
                //}
                //return BadRequest();
            }
            return Unauthorized();



        }


        //public IActionResult Login()
        //{
        //    return View();
        //}





        //// GET: UserController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: UserController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: UserController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: UserController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: UserController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: UserController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: UserController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
