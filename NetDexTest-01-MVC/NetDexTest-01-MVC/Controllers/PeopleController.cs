using Microsoft.AspNetCore.Mvc;
using System.Net;
using NetDexTest_01_MVC.Models;
using NetDexTest_01_MVC.Models.ViewModels;
using NetDexTest_01_MVC.Models.Entities;
using NetDexTest_01_MVC.Models.Authentication;
using NetDexTest_01_MVC.Services;
using Microsoft.Docs.Samples;

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


        [HttpGet("list", Name = "DexListDestination")]
        public async Task<IActionResult> ListDexIndex()
        {
            await Console.Out.WriteLineAsync($"\n\n\n\n\n\n\n--GET---ListDexIndex()----ACCESSING------------\n");
            await Console.Out.WriteLineAsync($"\n\n--GET---ListDexIndex()----Checking tmpData---\n\n\t{TempData["tEmail"]}");
            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");
            
            if (await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator", "User"))
            {

                //var req = _userSessionService.GetEmail();
                //var dex = await _personService.GetDexHolderMiddleVMAsync(req);

                await Console.Out.WriteLineAsync($"\n\n--GET---ListDexIndex()----Checking tmpData---\n\n\t{TempData["tEmail"]}");
                await Console.Out.WriteLineAsync($"\n\n--GET---ListDexIndex()----Checking userTmp---\n\n\t{_userSessionService.GetTempEmail()}");
                if (TempData["tEmail"] != null || _userSessionService.GetTempEmail() != null)
                {
                    if (TempData["tEmail"] != null) { ViewData["LoggedInEmail"] = TempData["tEmail"]; }
                    if (_userSessionService.GetTempEmail() != null) { _userSessionService.GetTempEmail(); }
                }
                else
                {
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();

                }

                ////ICollection<AdminUserVM> userList = await _userService.GetAllUsersAdminAsync();
                ////if(userList != null )
                ////{
                //return View(dex);// userList);
                //}
                //return BadRequest();

                return View();
            }
            return Unauthorized("You must be logged into an authorized account to view this page!");

            //return View();
        }


        [Route("u/{id}")]
        [HttpGet(Name = "DexIndexSingle")]
        public async Task<IActionResult> ListDexById(string id)
        {
            await Console.Out.WriteLineAsync($"\n\n--GET---ListDexIndex(id)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---ListDexIndex(id)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---ListDexIndex(id)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(id);
            await Console.Out.WriteLineAsync($"\n\n--GET---ListDexIndex(id)----currentUserFlag---\n\t{currentUserFlag}");

            bool userExists = await _authService.CheckIfUserExistsAsync(id);
            await Console.Out.WriteLineAsync($"\n\n--GET---ListDexIndex(id)------userExists?-----\n\t{userExists}");

            var url = Url.RouteUrl("DexListDestination");
            // if the target exists...
            if (userExists)
            {
                if (userFlag && currentUserFlag)
                {
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail(); 
                    //return RedirectToAction("ListDexIndex", "People");
                    return RedirectToRoute("DexListDestination");
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    await _userSessionService.CloseTempSessionData();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                    return RedirectToRoute("DexListDestination");
                    //return RedirectToAction("Index", "Home", new { id = 2 });
                    //return View();


                    // TODO : check if the input matches the logged in user
                }
                return Unauthorized("You cannot access another User's page, unless you are using an Administrator or Moderator's account!!");
            }
            else
            {
                return BadRequest("That user does not exist!");
            }


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
