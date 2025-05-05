using Microsoft.AspNetCore.Mvc;
using NetDexTest_01_MVC.Models.ViewModels;
using NetDexTest_01_MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using NetDexTest_01_MVC.Models;
using NetDexTest_01_MVC.Models.Entities;
using NetDexTest_01_MVC.Models.Authentication;
using Microsoft.Docs.Samples;
using toolExtensions;
using System.Threading.Tasks;

namespace NetDexTest_01_MVC.Controllers
    {
        [Route("dex")]
        public class EntryItemController : Controller
        {
            private readonly IUserSessionService _userSessionService;
            private readonly IPersonService _personService;
            private readonly ILogger<EntryItemController> _logger;
            private IAuthService _authService;
            private readonly IUserService _userService;
            private readonly IApiCallerService _apiCallerService;

            public EntryItemController(ILogger<EntryItemController> logger,
                IAuthService authService,
                IApiCallerService apiCallerService,
                IUserService userService,
                IUserSessionService userSessionService, IPersonService personService)
            {
                _logger = logger;
                _authService = authService;
                _apiCallerService = apiCallerService;
                _userService = userService;
                _userSessionService = userSessionService;
                _personService = personService;
            }


            
            [Route("u/{userId}/p/{personId}/rec/ie/{entryItemId}")]
            [HttpGet(Name = "EntryItemDetails")]
            public async Task<IActionResult> GetEntryItemDetailedView(string userId, string personId, long entryItemId)
            {
                await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)----ACCESSING---------\n");

                if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

                bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
                await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)-----------roleFlag---\n\t{roleFlag}");
                bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
                await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)-----------userFlag---\n\t{userFlag}");
                bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
                await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)----currentUserFlag---\n\t{currentUserFlag}");

                //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
                //await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)------userExists?-----\n\t{userExists}");
                bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
                await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)------userPersonExists?-----\n\t{userPersonExists}");

                //var url = Url.RouteUrl("DexListDestination");
                if (userPersonExists)
                {
                    if (userFlag && currentUserFlag)
                    {
                        await _userSessionService.SetTempPersonAsync(personId);

                        TempData["LastPerson"] = _userSessionService.GetTempPerson();
                        ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                        // we're working with the confirmed current user, so just get the email that's stored in user session service
                        ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                        TempData["tEmail"] = _userSessionService.GetEmail();
                        //return RedirectToAction("GetEntryItemDetailedView", "People");
                        return View();
                        //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                    }
                    else if (roleFlag)
                    {

                        await _userSessionService.SetTempPersonAsync(personId);
                        await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(userId)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                        ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                        TempData["tEmail"] = _userSessionService.GetTempEmail();
                        TempData["LastPerson"] = _userSessionService.GetTempPerson();
                        ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                        TempData["entryId"] = entryItemId;
                        ViewData["entryId"] = entryItemId;


                        // HACK - hopefully this shouldnt need to include anything about record collector


                        // await _userSessionService.SetTempPersonAsync(personId);
                        // ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                        // TempData["tEmail"] = _userSessionService.GetTempEmail();
                        // TempData["LastPerson"] = _userSessionService.GetTempPerson();
                        // ViewData["LastPerson"] = _userSessionService.GetTempPerson();


                        return View();
                        // TODO : check if the input matches the logged in user
                    }
                    return Unauthorized("You cannot access another User's page, unless you are using an Administrator or Moderator's account!!");
                }
            else
            {
                return BadRequest("That user does not exist!");
            }
        }




        [Route("u/{userId}/p/{personId}/edit/ie/{entryItemId}")]
            [HttpGet(Name = "EntryItemEdit")]
            public async Task<IActionResult> EditEntryItemView(string userId, string personId, long entryItemId)
            {
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
            //await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)------userPersonExists?-----\n\t{userPersonExists}");

            //var url = Url.RouteUrl("DexListDestination");
            if (userPersonExists)
            {
                if (userFlag && currentUserFlag)
                {
                    await _userSessionService.SetTempPersonAsync(personId);

                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail();
                    //return RedirectToAction("GetEntryItemDetailedView", "People");
                    return View();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    await _userSessionService.SetTempPersonAsync(personId);
                    await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(userId)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    TempData["entryId"] = entryItemId;
                    ViewData["entryId"] = entryItemId;


                    // HACK - hopefully this shouldnt need to include anything about record collector


                    // await _userSessionService.SetTempPersonAsync(personId);
                    // ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    // TempData["tEmail"] = _userSessionService.GetTempEmail();
                    // TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    // ViewData["LastPerson"] = _userSessionService.GetTempPerson();


                    return View();
                    // TODO : check if the input matches the logged in user
                }
                return Unauthorized("You cannot access another User's page, unless you are using an Administrator or Moderator's account!!");
            }
            else
            {
                return BadRequest("That user does not exist!");
            }
        }





        [Route("u/{userId}/p/{personId}/delete/ie/{entryItemId}")]
            [HttpGet(Name = "EntryItemDelete")]
            public async Task<IActionResult> DeleteEntryItemView(string userId, string personId, long entryItemId)
            {
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
            //await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)------userPersonExists?-----\n\t{userPersonExists}");

            //var url = Url.RouteUrl("DexListDestination");
            if (userPersonExists)
            {
                if (userFlag && currentUserFlag)
                {
                    await _userSessionService.SetTempPersonAsync(personId);

                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail();
                    //return RedirectToAction("GetEntryItemDetailedView", "People");
                    return View();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    await _userSessionService.SetTempPersonAsync(personId);
                    await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(userId)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    TempData["entryId"] = entryItemId;
                    ViewData["entryId"] = entryItemId;


                    // HACK - hopefully this shouldnt need to include anything about record collector


                    // await _userSessionService.SetTempPersonAsync(personId);
                    // ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    // TempData["tEmail"] = _userSessionService.GetTempEmail();
                    // TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    // ViewData["LastPerson"] = _userSessionService.GetTempPerson();


                    return View();
                    // TODO : check if the input matches the logged in user
                }
                return Unauthorized("You cannot access another User's page, unless you are using an Administrator or Moderator's account!!");
            }
            else
            {
                return BadRequest("That user does not exist!");
            }
        }

        [Route("u/{userId}/p/{personId}/create/rec/ie")]
            [HttpGet(Name = "EntryItemCreate")]
            public async Task<IActionResult> CreateEntryItemView(string userId, string personId)
            {
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
            //await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)------userPersonExists?-----\n\t{userPersonExists}");

            //var url = Url.RouteUrl("DexListDestination");
            if (userPersonExists)
            {
                if (userFlag && currentUserFlag)
                {
                    await _userSessionService.SetTempPersonAsync(personId);

                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail();
                    //return RedirectToAction("GetEntryItemDetailedView", "People");
                    return View();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    await _userSessionService.SetTempPersonAsync(personId);
                    await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(userId)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();


                    // HACK - hopefully this shouldnt need to include anything about record collector


                    // await _userSessionService.SetTempPersonAsync(personId);
                    // ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    // TempData["tEmail"] = _userSessionService.GetTempEmail();
                    // TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    // ViewData["LastPerson"] = _userSessionService.GetTempPerson();


                    return View();
                    // TODO : check if the input matches the logged in user
                }
                return Unauthorized("You cannot access another User's page, unless you are using an Administrator or Moderator's account!!");
            }
            else
            {
                return BadRequest("That user does not exist!");
            }
        }




        [Route("u/{userId}/p/{personId}/rec/list/ie")]
            [HttpGet(Name = "EntryItemList")]
            public async Task<IActionResult> ListEntryItemsView(string userId, string personId)
            {
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
            //await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
            await Console.Out.WriteLineAsync($"\n\n--GET---GetEntryItemDetailedView(userId)------userPersonExists?-----\n\t{userPersonExists}");

            //var url = Url.RouteUrl("DexListDestination");
            if (userPersonExists)
            {
                if (userFlag && currentUserFlag)
                {
                    await _userSessionService.SetTempPersonAsync(personId);

                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail();
                    //return RedirectToAction("GetEntryItemDetailedView", "People");
                    return View();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    await _userSessionService.SetTempPersonAsync(personId);
                    await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(userId)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // TempData["entryId"] = entryItemId;
                    // ViewData["entryId"] = entryItemId;


                    // HACK - hopefully this shouldnt need to include anything about record collector


                    // await _userSessionService.SetTempPersonAsync(personId);
                    // ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    // TempData["tEmail"] = _userSessionService.GetTempEmail();
                    // TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    // ViewData["LastPerson"] = _userSessionService.GetTempPerson();


                    return View();
                    // TODO : check if the input matches the logged in user
                }
                return Unauthorized("You cannot access another User's page, unless you are using an Administrator or Moderator's account!!");
            }
            else
            {
                return BadRequest("That user does not exist!");
            }
        }





    }
}



    #region oldScaffold

        // namespace NetDexTest_01_MVC.Controllers
        // {
        //     [Route("entryitem")]
        //     public class EntryItemController : Controller
        //     {
        //         private readonly IEntryItemService _entryItemService;

        //         public EntryItemController(IEntryItemService entryItemService)
        //         {
        //             _entryItemService = entryItemService;
        //         }

        // [HttpGet("list")]
        // public async Task<IActionResult> List()
        // {
        //     var items = await _entryItemService.GetAllAsync();
        //     return View(items);
        // }

        // [HttpGet("details/{id}")]
        // public async Task<IActionResult> Details(long id)
        // {
        //     var item = await _entryItemService.GetByIdAsync(id);
        //     return View(item);
        // }

        // [HttpGet("create")]
        // public IActionResult Create()
        // {
        //     return View();
        // }

        // [HttpPost("create")]
        // public async Task<IActionResult> Create(EntryItemVM model)
        // {
        //     if (!ModelState.IsValid) return View(model);

        //     var success = await _entryItemService.CreateAsync(model);
        //     if (!success) return BadRequest("Failed to create entry.");
        //     return RedirectToAction("List");
        // }

        // [HttpGet("edit/{id}")]
        // public async Task<IActionResult> Edit(long id)
        // {
        //     var item = await _entryItemService.GetByIdAsync(id);
        //     return View(item);
        // }

        // [HttpPost("edit")]
        // public async Task<IActionResult> Edit(EntryItemVM model)
        // {
        //     if (!ModelState.IsValid) return View(model);

        //     var success = await _entryItemService.UpdateAsync(model);
        //     if (!success) return BadRequest("Failed to update entry.");
        //     return RedirectToAction("List");
        // }

        // [HttpPost("delete/{id}")]
        // public async Task<IActionResult> Delete(long id)
        // {
        //     var success = await _entryItemService.DeleteAsync(id);
        //     if (!success) return BadRequest("Failed to delete entry.");
        //     return RedirectToAction("List");
        // }


        #endregion oldScaffold
    

