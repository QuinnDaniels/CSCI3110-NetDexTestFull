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
    public class SocialMediaController : Controller
    {
        private readonly IUserSessionService _userSessionService;
        private readonly IPersonService _personService;
        private readonly ILogger<SocialMediaController> _logger;
        private IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ISocialMediaService _socialmediaService;
        private readonly IApiCallerService _apiCallerService;

        public SocialMediaController(ILogger<SocialMediaController> logger,
            IAuthService authService,
            IApiCallerService apiCallerService,
            IUserService userService,
            ISocialMediaService socialmediaService,
            IUserSessionService userSessionService, IPersonService personService)
        {
            _logger = logger;
            _authService = authService;
            _apiCallerService = apiCallerService;
            _socialmediaService = socialmediaService;
            _userService = userService;
            _userSessionService = userSessionService;
            _personService = personService;
        }


        #region ReadAll

        [Route("u/{userId}/p/{personId}/cont/list/soc")]
        [HttpGet(Name = "SocialMediaList")]
        public async Task<IActionResult> ListSocialMediasView(string userId, string personId)
        {
            await Console.Out.WriteLineAsync($"\n\n--GET---ListSocialMediasView(userId)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---ListSocialMediasView(userId)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---ListSocialMediasView(userId)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
            await Console.Out.WriteLineAsync($"\n\n--GET---ListSocialMediasView(userId)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
            //await Console.Out.WriteLineAsync($"\n\n--GET---ListSocialMediasView(userId)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
            await Console.Out.WriteLineAsync($"\n\n--GET---ListSocialMediasView(userId)------userPersonExists?-----\n\t{userPersonExists}");

            //var url = Url.RouteUrl("DexListDestination");
            if (userPersonExists)
            {
                if (userFlag && currentUserFlag)
                {
                    await _userSessionService.SetTempPersonAsync(personId);

                    var tempPerson = await _personService.GetPersonPlusDexListVMAsync(userId, personId);


                    ViewData["personNickname"] = tempPerson?.Nickname ?? "tempPerson was null in SocialMediasController.ListSocialMediasView";

                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail();
                    //return RedirectToAction("ListSocialMediasView", "People");
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
                    ViewData["personNickname"] = tempPerson?.Nickname ?? "tempPerson was null in SocialMediasController.ListSocialMediasView";

                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    TempData["tUsername"] = _userSessionService.GetUsername();
                    ViewData["tUsername"] = _userSessionService.GetUsername();
                    // TempData["socialmediaId"] = socialMediaId.ToString();
                    // ViewData["socialmediaId"] = socialMediaId.ToString();


                    // HACK - hopefully this shouldnt need to include anything about contact info


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

        [Route("u/{userId}/p/{personId}/cont/soc/{socialMediaId}")]
        [HttpGet(Name = "SocialMediaDetails")]
        public async Task<IActionResult> GetSocialMediaDetailedView(string userId, string personId, long socialMediaId)
        {
            await Console.Out.WriteLineAsync($"\n\n--GET---GetSocialMediaDetailedView(userId)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---GetSocialMediaDetailedView(userId)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---GetSocialMediaDetailedView(userId)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
            await Console.Out.WriteLineAsync($"\n\n--GET---GetSocialMediaDetailedView(userId)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
            //await Console.Out.WriteLineAsync($"\n\n--GET---GetSocialMediaDetailedView(userId)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
            await Console.Out.WriteLineAsync($"\n\n--GET---GetSocialMediaDetailedView(userId)------userPersonExists?-----\n\t{userPersonExists}");

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
                    TempData["socialmediaId"] = socialMediaId.ToString();
                    ViewData["socialmediaId"] = socialMediaId.ToString();
                    //return RedirectToAction("GetSocialMediaDetailedView", "People");
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
                    TempData["socialmediaId"] = socialMediaId.ToString();
                    ViewData["socialmediaId"] = socialMediaId.ToString();




                    // HACK - hopefully this shouldnt need to include anything about contact info


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



        [Route("u/{userId}/p/{personId}/edit/soc/{socialMediaId}")]
        [HttpGet(Name = "SocialMediaEdit")]
        public async Task<IActionResult> EditSocialMediaView(string userId, string personId, long socialMediaId)
        {
            await Console.Out.WriteLineAsync($"\n\n--GET---EditSocialMediaView(userId)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---EditSocialMediaView(userId)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---EditSocialMediaView(userId)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
            await Console.Out.WriteLineAsync($"\n\n--GET---EditSocialMediaView(userId)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
            //await Console.Out.WriteLineAsync($"\n\n--GET---EditSocialMediaView(userId)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
            await Console.Out.WriteLineAsync($"\n\n--GET---EditSocialMediaView(userId)------userPersonExists?-----\n\t{userPersonExists}");

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
                    TempData["socialmediaId"] = socialMediaId.ToString();
                    ViewData["socialmediaId"] = socialMediaId.ToString();

                    try
                    {
                        SocialMediaVM? socialmediaVM = null;
                        socialmediaVM = await _socialmediaService.GetByIdAsync(socialMediaId);
                        if (socialmediaVM == null) return NotFound("An socialmedia could not be found for the specified Int64 socialMediaId");
                        ViewData["oldCategory"] = socialmediaVM.CategoryField;
                        ViewData["oldHandle"] = socialmediaVM.SocialHandle;
                        TempData["oldCategory"] = socialmediaVM.CategoryField;
                        TempData["oldHandle"] = socialmediaVM.SocialHandle;
                    }
                    catch (Exception ex)
                    {
                        await Console.Out.WriteLineAsync($"\n\n---------EditSocialMediaView(str, str, long)------Using _socialmediaService.GetByIdAsync(long)\n\t ERROR: resulted in \"{ex.Message}\"\n\n-----------------------");
                        throw;
                    }


                    try
                    {
                        var person = await _personService.GetPersonPlusDexListVMAsync(_userSessionService.GetEmail(), personId);
                        if (person == null)
                        {
                            return NotFound();
                        }
                        await _userSessionService.SetTempPersonAsync(person.ContactInfoId, person.ContactInfoId);
                        TempData["tContactInfoId"] = _userSessionService.GetTempContactInfo();
                        //TempData["contactInfoId"] = _userSessionService.GetTempContactInfo();
                        ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();
                        //ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();

                        return View();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in SocialMedia getting PersonPlusDex view endpoint: " + ex.Message);
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
                    TempData["socialmediaId"] = socialMediaId.ToString();
                    ViewData["socialmediaId"] = socialMediaId.ToString();


                    try
                    {
                        SocialMediaVM? socialmediaVM = null;
                        socialmediaVM = await _socialmediaService.GetByIdAsync(socialMediaId);
                        if (socialmediaVM == null) return NotFound("An socialmedia could not be found for the specified Int64 socialMediaId");
                        TempData["oldCategory"] = socialmediaVM.CategoryField;
                        TempData["oldHandle"] = socialmediaVM.SocialHandle;
                    }
                    catch (Exception ex)
                    {
                        await Console.Out.WriteLineAsync($"\n\n---------EditSocialMediaView(str, str, long)------Using _socialmediaService.GetByIdAsync(long)\n\t ERROR: resulted in \"{ex.Message}\"\n\n-----------------------");
                        throw;
                    }

                    try
                    {
                        var person = await _personService.GetPersonPlusDexListVMAsync(_userSessionService.GetEmail(), personId);
                        if (person == null)
                        {
                            return NotFound();
                        }
                        await _userSessionService.SetTempPersonAsync(person.ContactInfoId, person.ContactInfoId);
                        TempData["tContactInfoId"] = _userSessionService.GetTempContactInfo();
                        //TempData["contactInfoId"] = _userSessionService.GetTempContactInfo();
                        ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();
                        //ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();

                        return View();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in SocialMedia getting PersonPlusDex view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for viewing.");
                    }


                    // HACK - hopefully this shouldnt need to include anything about contact info


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



        [Route("u/{userId}/p/{personId}/delete/soc/{socialMediaId}")]
        [HttpGet(Name = "SocialMediaDelete")]
        public async Task<IActionResult> DeleteSocialMediaView(string userId, string personId, long socialMediaId)
        {
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteSocialMediaView(userId)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteSocialMediaView(userId)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteSocialMediaView(userId)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteSocialMediaView(userId)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
            //await Console.Out.WriteLineAsync($"\n\n--GET---DeleteSocialMediaView(userId)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteSocialMediaView(userId)------userPersonExists?-----\n\t{userPersonExists}");

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
                    //return RedirectToAction("DeleteSocialMediaView", "People");
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
                    // HACK - hopefully this shouldnt need to include anything about contact info
                    // NOTE - UPDATE: It totally fucking did!!!!
                    TempData["socialmediaId"] = socialMediaId.ToString();
                    ViewData["socialmediaId"] = socialMediaId.ToString();
                    try
                    {
                        var person = await _personService.GetPersonPlusDexListVMAsync(_userSessionService.GetEmail(), personId);
                        if (person == null)
                        {
                            return NotFound();
                        }
                        await _userSessionService.SetTempPersonAsync(person.ContactInfoId, person.ContactInfoId);
                        TempData["tContactInfoId"] = _userSessionService.GetTempContactInfo();
                        //TempData["contactInfoId"] = _userSessionService.GetTempContactInfo();
                        ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();
                        //ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();

                        return View();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in SocialMedia getting PersonPlusDex view endpoint: " + ex.Message);
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



        [Route("u/{userId}/p/{personId}/create/cont/soc")]
        [HttpGet(Name = "SocialMediaCreate")]
        public async Task<IActionResult> CreateSocialMediaView(string userId, string personId)
        {
            await Console.Out.WriteLineAsync($"\n\n--GET---CreateSocialMediaView(userId)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---CreateSocialMediaView(userId)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---CreateSocialMediaView(userId)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(userId);
            await Console.Out.WriteLineAsync($"\n\n--GET---CreateSocialMediaView(userId)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(userId);
            //await Console.Out.WriteLineAsync($"\n\n--GET---CreateSocialMediaView(userId)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(userId);//, personId);
            await Console.Out.WriteLineAsync($"\n\n--GET---CreateSocialMediaView(userId)------userPersonExists?-----\n\t{userPersonExists}");

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
                        await _userSessionService.SetTempPersonAsync(person.ContactInfoId, person.ContactInfoId);
                        TempData["tContactInfoId"] = _userSessionService.GetTempContactInfo();
                        //TempData["contactInfoId"] = _userSessionService.GetTempContactInfo();
                        ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();
                        //ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();
                        return View();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in SocialMedia getting PersonPlusDex view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for viewing.");
                    }

                    //return RedirectToAction("GetSocialMediaDetailedView", "People");
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
                        await _userSessionService.SetTempPersonAsync(person.ContactInfoId, person.ContactInfoId);
                        TempData["ContactInfoId"] = _userSessionService.GetTempContactInfo();
                        //TempData["contactInfoId"] = _userSessionService.GetTempContactInfo();
                        ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();
                        //ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();
                        return View();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in SocialMedia getting PersonPlusDex view endpoint: " + ex.Message);
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
//     [Route("socialmedia")]
//     public class SocialMediaController : Controller
//     {
//         private readonly ISocialMediaService _socialMediaService;

//         public SocialMediaController(ISocialMediaService socialMediaService)
//         {
//             _socialMediaService = socialMediaService;
//         }

// [HttpGet("list")]
// public async Task<IActionResult> List()
// {
//     var items = await _socialMediaService.GetAllAsync();
//     return View(items);
// }

// [HttpGet("details/{id}")]
// public async Task<IActionResult> Details(long id)
// {
//     var item = await _socialMediaService.GetByIdAsync(id);
//     return View(item);
// }

// [HttpGet("create")]
// public IActionResult Create()
// {
//     return View();
// }

// [HttpPost("create")]
// public async Task<IActionResult> Create(SocialMediaVM model)
// {
//     if (!ModelState.IsValid) return View(model);

//     var success = await _socialMediaService.CreateAsync(model);
//     if (!success) return BadRequest("Failed to create socialmedia.");
//     return RedirectToAction("List");
// }

// [HttpGet("edit/{id}")]
// public async Task<IActionResult> Edit(long id)
// {
//     var item = await _socialMediaService.GetByIdAsync(id);
//     return View(item);
// }

// [HttpPost("edit")]
// public async Task<IActionResult> Edit(SocialMediaVM model)
// {
//     if (!ModelState.IsValid) return View(model);

//     var success = await _socialMediaService.UpdateAsync(model);
//     if (!success) return BadRequest("Failed to update socialmedia.");
//     return RedirectToAction("List");
// }

// [HttpPost("delete/{id}")]
// public async Task<IActionResult> Delete(long id)
// {
//     var success = await _socialMediaService.DeleteAsync(id);
//     if (!success) return BadRequest("Failed to delete socialmedia.");
//     return RedirectToAction("List");
// }


#endregion oldScaffold


