using Microsoft.AspNetCore.Mvc;
using System.Net;
using NetDexTest_01_MVC.Models;
using NetDexTest_01_MVC.Models.ViewModels;
using NetDexTest_01_MVC.Models.Entities;
using NetDexTest_01_MVC.Models.Authentication;
using NetDexTest_01_MVC.Services;
using Microsoft.Docs.Samples;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace NetDexTest_01_MVC.Controllers
{
    [Route("dex")]
    public class RelationshipController : Controller
    {
        private readonly IUserSessionService _userSessionService;
        private readonly IPersonService _personService;
        private readonly ILogger<AuthController> _logger;
        private IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IApiCallerService _apiCallerService;


        public RelationshipController(ILogger<AuthController> logger,
            IAuthService authService,
            IApiCallerService apiCallerService,
            IUserService userService,
            IPersonService personService,
            IUserSessionService userSessionService)
        {
            _personService = personService;
            _userSessionService = userSessionService;
            _logger = logger;
            _authService = authService;
            _apiCallerService = apiCallerService;
            _userService = userService;

        }

        //[HttpGet("u/{userId}/relations/list")] //p/{personId}/rel/list")]
        //public async Task<IActionResult> ListRelationships(string userId, string personId)
        //{
        //    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
        //    TempData["LastPerson"] = personId;

        //    var results = await _personService.GetRelationshipsForUserAsync(userId);
        //    return View("RelationshipListView", results);
        //}


        [HttpGet("u/{userId}/relations/list")] //p/{personId}/rel/list")]
        public async Task<IActionResult> ListRelationships(string userId)
        {
            await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(id);
            await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(id);
            //await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(id);//, criteria);
            await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)------userPersonExists?-----\n\t{userPersonExists}");

            var url = Url.RouteUrl("DexListDestination");
            // if the target exists...
            //if (userExists)
            if (userPersonExists)
            {
                if (userFlag && currentUserFlag)
                {

                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail();
                    //return RedirectToAction("DetailsViewByRoute", "People");



                    try
                    {

                        return View(); // assumes you have Views/People/DeletePerson.cshtml
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in DetailsPerson view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for viewing.");
                    }

                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    // HACK disbling for now for if an admin goes to another user to create a person
                    //await _userSessionService.CloseTempSessionData();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");


                    return View();
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
        }





        [HttpGet("u/{userId}/relations/create")] //p/{personId}/rel/create")]
        public IActionResult CreateRelationship()
        {
            ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
            TempData["LastPerson"] = _userSessionService.GetTempPerson();
            return View("CreateRelationshipView");
        }

        //[HttpPost("u/{userId}/p/{personId}/rel/create")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateRelationship(RelationshipRequest request)
        //{
        //    if (!ModelState.IsValid)
        //        return View(request);

        //    var response = await _personService.CreateRelationshipAsync(request);
        //    if (response.Status == HttpStatusCode.OK)
        //        return RedirectToAction("ListRelationships", new { userId = request.AppUserInput, personId = request.PersonNickname });

        //    ModelState.AddModelError("", "Unable to create relationship.");
        //    return View(request);
        //}








    }
}
