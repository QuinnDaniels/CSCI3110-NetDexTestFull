﻿using Microsoft.AspNetCore.Mvc;
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
            private readonly IEntryItemService _entryService;
            private readonly IApiCallerService _apiCallerService;

            public EntryItemController(ILogger<EntryItemController> logger,
                IAuthService authService,
                IApiCallerService apiCallerService,
                IUserService userService,
                IEntryItemService entryService,
                IUserSessionService userSessionService, IPersonService personService)
            {
                _logger = logger;
                _authService = authService;
                _apiCallerService = apiCallerService;
                _entryService = entryService;
                _userService = userService;
                _userSessionService = userSessionService;
                _personService = personService;
            }


        #region ReadAll

        [Route("u/{userId}/p/{personId}/rec/list/ie")]
        [HttpGet(Name = "EntryItemList")]
        public async Task<IActionResult> ListEntryItemsView(string userId, string personId)
        {
            await Console.Out.WriteLineAsync($"\n\n--GET---ListEntryItemsView(userId)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---ListEntryItemsView(userId)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---ListEntryItemsView(userId)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
            await Console.Out.WriteLineAsync($"\n\n--GET---ListEntryItemsView(userId)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
            //await Console.Out.WriteLineAsync($"\n\n--GET---ListEntryItemsView(userId)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
            await Console.Out.WriteLineAsync($"\n\n--GET---ListEntryItemsView(userId)------userPersonExists?-----\n\t{userPersonExists}");

            //var url = Url.RouteUrl("DexListDestination");
            if (userPersonExists)
            {
                if (userFlag && currentUserFlag)
                {
                    await _userSessionService.SetTempPersonAsync(personId);

                    var tempPerson = await _personService.GetPersonPlusDexListVMAsync(userId, personId);


                    ViewData["personNickname"] = tempPerson?.Nickname ?? "tempPerson was null in EntryItemsController.ListEntryItemsView";

                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail();
                    //return RedirectToAction("ListEntryItemsView", "People");
                    TempData["tUsername"] = _userSessionService.GetUsername();
                    ViewData["tUsername"] = _userSessionService.GetUsername();

                    return View();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    await _userSessionService.SetTempPersonAsync(personId);
                    await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(userId)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    var tempPerson = await _personService.GetPersonPlusDexListVMAsync(userId, personId);
                    ViewData["personNickname"] = tempPerson?.Nickname ?? "tempPerson was null in EntryItemsController.ListEntryItemsView";

                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    TempData["tUsername"] = _userSessionService.GetUsername();
                    ViewData["tUsername"] = _userSessionService.GetUsername();
                    // TempData["entryId"] = entryItemId.ToString();
                    // ViewData["entryId"] = entryItemId.ToString();


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
        #endregion ReadAll


        #region  ReadOne
            
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
                        await Console.Out.WriteLineAsync($"\n++++++++++++\n\temail:\t{_userSessionService.GetTempEmail()}");
                        // we're working with the confirmed current user, so just get the email that's stored in user session service
                        ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                        TempData["LoggedInEmail"] = _userSessionService.GetEmail();
                        TempData["tEmail"] = _userSessionService.GetEmail();
                        TempData["tUsername"] = _userSessionService.GetUsername();
                        ViewData["tUsername"] = _userSessionService.GetUsername();
                        TempData["entryId"] = entryItemId.ToString();
                        ViewData["entryId"] = entryItemId.ToString();
                        //return RedirectToAction("GetEntryItemDetailedView", "People");
                        return View();
                        //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");




                    }
                    else if (roleFlag)
                    {

                        await _userSessionService.SetTempPersonAsync(personId);
                        await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(userId)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                        
                        await Console.Out.WriteLineAsync($"\n++++++++++++++++++\n\temail:\t{_userSessionService.GetTempEmail()}");
                        ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                        TempData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                        TempData["tEmail"] = _userSessionService.GetTempEmail();
                        TempData["LastPerson"] = _userSessionService.GetTempPerson();
                        ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                        ViewData["tUsername"] = _userSessionService.GetUsername();
                        TempData["tUsername"] = _userSessionService.GetUsername();
                        TempData["entryId"] = entryItemId.ToString();
                        ViewData["entryId"] = entryItemId.ToString();




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

        #endregion ReadOne
        #region Edit
            


        [Route("u/{userId}/p/{personId}/edit/ie/{entryItemId}")]
            [HttpGet(Name = "EntryItemEdit")]
            public async Task<IActionResult> EditEntryItemView(string userId, string personId, long entryItemId)
            {
            await Console.Out.WriteLineAsync($"\n\n--GET---EditEntryItemView(userId)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---EditEntryItemView(userId)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---EditEntryItemView(userId)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
            await Console.Out.WriteLineAsync($"\n\n--GET---EditEntryItemView(userId)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
            //await Console.Out.WriteLineAsync($"\n\n--GET---EditEntryItemView(userId)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
            await Console.Out.WriteLineAsync($"\n\n--GET---EditEntryItemView(userId)------userPersonExists?-----\n\t{userPersonExists}");

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
                    TempData["entryId"] = entryItemId.ToString();
                    ViewData["entryId"] = entryItemId.ToString();

                    try
                    {
                        EntryItemVM? entryVM = null;
                        entryVM = await _entryService.GetByIdAsync(entryItemId);
                        if (entryVM == null) return NotFound("An entry could not be found for the specified Int64 entryItemId");
                        ViewData["oldTitle"] = entryVM.ShortTitle;
                        ViewData["oldFlavor"] = entryVM.FlavorText;
                        TempData["oldTitle"] = entryVM.ShortTitle;
                        TempData["oldFlavor"] = entryVM.FlavorText;
                    }
                    catch (Exception ex)
                    {
                        await Console.Out.WriteLineAsync($"\n\n---------EditEntryItemView(str, str, long)------Using _entryService.GetByIdAsync(long)\n\t ERROR: resulted in \"{ex.Message}\"\n\n-----------------------");
                        throw;
                    }


                    try {
                        var person = await _personService.GetPersonPlusDexListVMAsync(_userSessionService.GetEmail(), personId);
                        if (person == null) {
                            return NotFound();
                        }
                        await _userSessionService.SetTempPersonAsync(person.RecordCollectorId, person.ContactInfoId);
                        TempData["tRecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        //TempData["tContactInfoId"] = _userSessionService.GetTempContactInfo();
                        ViewData["RecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        //ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();

                        return View();
                    } catch (Exception ex) {
                        Console.WriteLine("Error in EntryItem getting PersonPlusDex view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for viewing.");
                    }
                }
                else if (roleFlag)
                {

                    await _userSessionService.SetTempPersonAsync(personId);
                    await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(userId)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    TempData["entryId"] = entryItemId.ToString();
                    ViewData["entryId"] = entryItemId.ToString();


                    try
                    {
                        EntryItemVM? entryVM = null;
                        entryVM = await _entryService.GetByIdAsync(entryItemId);
                        if (entryVM == null) return NotFound("An entry could not be found for the specified Int64 entryItemId");
                        TempData["oldTitle"] = entryVM.ShortTitle;
                        TempData["oldFlavor"] = entryVM.FlavorText;
                    }
                    catch (Exception ex)
                    {
                        await Console.Out.WriteLineAsync($"\n\n---------EditEntryItemView(str, str, long)------Using _entryService.GetByIdAsync(long)\n\t ERROR: resulted in \"{ex.Message}\"\n\n-----------------------");
                        throw;
                    }

                    try
                    {
                        var person = await _personService.GetPersonPlusDexListVMAsync(_userSessionService.GetEmail(), personId);
                        if (person == null)
                        {
                            return NotFound();
                        }
                        await _userSessionService.SetTempPersonAsync(person.RecordCollectorId, person.ContactInfoId);
                        TempData["tRecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        //TempData["tContactInfoId"] = _userSessionService.GetTempContactInfo();
                        ViewData["RecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        //ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();

                        return View(); 
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in EntryItem getting PersonPlusDex view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for viewing.");
                    }


                    // HACK - hopefully this shouldnt need to include anything about record collector


                    // await _userSessionService.SetTempPersonAsync(personId);
                    // ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    // TempData["tEmail"] = _userSessionService.GetTempEmail();
                    // TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    // ViewData["LastPerson"] = _userSessionService.GetTempPerson();


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


        #endregion Edit
        #region Delete
            


        [Route("u/{userId}/p/{personId}/delete/ie/{entryItemId}")]
            [HttpGet(Name = "EntryItemDelete")]
            public async Task<IActionResult> DeleteEntryItemView(string userId, string personId, long entryItemId)
            {
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteEntryItemView(userId)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteEntryItemView(userId)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteEntryItemView(userId)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteEntryItemView(userId)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
            //await Console.Out.WriteLineAsync($"\n\n--GET---DeleteEntryItemView(userId)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteEntryItemView(userId)------userPersonExists?-----\n\t{userPersonExists}");

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
                    TempData["tUsername"] = _userSessionService.GetUsername();
                    ViewData["tUsername"] = _userSessionService.GetUsername();
                    TempData["tEmail"] = _userSessionService.GetEmail();
                    //return RedirectToAction("DeleteEntryItemView", "People");
                    return View();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    await _userSessionService.SetTempPersonAsync(personId);
                    await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(userId)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    ViewData["tUsername"] = _userSessionService.GetUsername();
                    TempData["tUsername"] = _userSessionService.GetUsername();
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // HACK - hopefully this shouldnt need to include anything about record collector
                    // NOTE - UPDATE: It totally fucking did!!!!
                    TempData["entryId"] = entryItemId.ToString();
                    ViewData["entryId"] = entryItemId.ToString();
                    try
                    {
                        var person = await _personService.GetPersonPlusDexListVMAsync(_userSessionService.GetEmail(), personId);
                        if (person == null)
                        {
                            return NotFound();
                        }
                        await _userSessionService.SetTempPersonAsync(person.RecordCollectorId, person.ContactInfoId);
                        TempData["tRecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        //TempData["tContactInfoId"] = _userSessionService.GetTempContactInfo();
                        ViewData["RecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        //ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();

                        return View();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in EntryItem getting PersonPlusDex view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for viewing.");
                    }
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
        #endregion Delete
        #region Create
            
            

        [Route("u/{userId}/p/{personId}/create/rec/ie")]
            [HttpGet(Name = "EntryItemCreate")]
            public async Task<IActionResult> CreateEntryItemView(string userId, string personId)
            {
            await Console.Out.WriteLineAsync($"\n\n--GET---CreateEntryItemView(userId)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---CreateEntryItemView(userId)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---CreateEntryItemView(userId)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
            await Console.Out.WriteLineAsync($"\n\n--GET---CreateEntryItemView(userId)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
            //await Console.Out.WriteLineAsync($"\n\n--GET---CreateEntryItemView(userId)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
            await Console.Out.WriteLineAsync($"\n\n--GET---CreateEntryItemView(userId)------userPersonExists?-----\n\t{userPersonExists}");

            //var url = Url.RouteUrl("DexListDestination");
            if (userPersonExists)
            {
                if (userFlag && currentUserFlag)
                {
                    await _userSessionService.SetTempPersonAsync(personId);

                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    TempData["tUsername"] = _userSessionService.GetUsername();
                    TempData["tUsername"] = _userSessionService.GetUsername();
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail();
                    try
                    {
                        var person = await _personService.GetPersonPlusDexListVMAsync(_userSessionService.GetEmail(), personId);
                        if (person == null)
                        {
                            return NotFound();
                        }
                        await _userSessionService.SetTempPersonAsync(person.RecordCollectorId, person.ContactInfoId);
                        TempData["tRecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        //TempData["tContactInfoId"] = _userSessionService.GetTempContactInfo();
                        ViewData["RecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        //ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();
                        return View();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in EntryItem getting PersonPlusDex view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for viewing.");
                    }

                    //return RedirectToAction("GetEntryItemDetailedView", "People");
                    return View();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    await _userSessionService.SetTempPersonAsync(personId);
                    await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(userId)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["tUsername"] = _userSessionService.GetUsername();
                    TempData["tUsername"] = _userSessionService.GetUsername();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    try
                    {
                        var person = await _personService.GetPersonPlusDexListVMAsync(_userSessionService.GetEmail(), personId);
                        if (person == null)
                        {
                            return NotFound();
                        }
                        await _userSessionService.SetTempPersonAsync(person.RecordCollectorId, person.ContactInfoId);
                        TempData["RecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        //TempData["tContactInfoId"] = _userSessionService.GetTempContactInfo();
                        ViewData["RecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        //ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();
                        return View();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in EntryItem getting PersonPlusDex view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for viewing.");
                    }
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

        #endregion Create










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
    

