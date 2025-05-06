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
    public class PeopleController : Controller
    {
        private readonly IUserSessionService _userSessionService;
        private readonly IPersonService _personService;
        private readonly ILogger<AuthController> _logger;
        private IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IApiCallerService _apiCallerService;


        public PeopleController(ILogger<AuthController> logger,
            IAuthService authService,
            IApiCallerService apiCallerService,
            IUserService userService,
            IPersonService personService,
            IUserSessionService userSessionService)
        {
            _logger = logger;
            _authService = authService;
            _apiCallerService = apiCallerService;
            _userService = userService;
            _userSessionService = userSessionService;
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

            //bool userExists = await _authService.CheckIfUserExistsAsync(id);
            //await Console.Out.WriteLineAsync($"\n\n--GET---ListDexIndex(id)------userExists?-----\n\t{userExists}");
            DexHolderMiddleVM? userExists = await _authService.CheckIfUserExistsReturnObjectAsync(id);
            await Console.Out.WriteLineAsync($"\n\n--GET---ListDexIndex(id)------userExists?-----\n\t{userExists}");

            var url = Url.RouteUrl("DexListDestination");
            // if the target exists...
            //if (userExists)
            if (userExists != null)
            {
                if (userFlag && currentUserFlag)
                {
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail(); 
                    //return RedirectToAction("ListDexIndex", "People");
                    //return RedirectToRoute("DexListDestination");
                    return View();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    await Console.Out.WriteLineAsync($"\n\n--GET---ListDexIndex(id)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    
                    // HACK disbling for now for if an admin goes to another user to create a person
                    //await _userSessionService.CloseTempSessionData();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                    
                    //return RedirectToAction("Index", "Home", new { id = 2 });
                    return View();


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
            await Console.Out.WriteLineAsync($"\n\n\n\n\n\n\n--GET---CreatePerson()----ACCESSING------------\n");
            await Console.Out.WriteLineAsync($"\n\n--GET---CreatePerson()----Checking tmpData---\n\n\t{TempData["tEmail"]}");
            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            await Console.Out.WriteLineAsync($"\n\n\n\n\n\n\n--GET---CreatePerson()--user is logged in...----\n");
            if (await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator", "User"))
            {
                await Console.Out.WriteLineAsync($"\n\n--GET---CreatePerson()----Checking tmpData---\n\n\t{TempData["tEmail"]}");
                await Console.Out.WriteLineAsync($"\n\n--GET---CreatePerson()----Checking userTmp---\n\n\t{_userSessionService.GetTempEmail()}");
                if (TempData["tEmail"] != null || _userSessionService.GetTempEmail() != null)
                {
                    await Console.Out.WriteLineAsync($"\n\n--GET---CreatePerson()----Setting LoggedInEmail---"
                                        + $"\n\t\tLoggedIn: {ViewData["LoggedInEmail"]}\n\t");
                    if (TempData["tEmail"] != null) {
                        await Console.Out.WriteAsync($"[IF1]:\t=> tEmail: {TempData["tEmail"]}\n\t");
                        ViewData["LoggedInEmail"] = TempData["tEmail"];
                        await Console.Out.WriteAsync($"\n\t\t=> LoggedIn: { ViewData["LoggedInEmail"]}\n\t");
                    }
                    if (_userSessionService.GetTempEmail() != null) {
                        ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                        await Console.Out.WriteAsync($"[IF2]:\t=> u.s.s.TempEmail: {_userSessionService.GetTempEmail()}\n\t");
                        await Console.Out.WriteAsync($"\n\t\t=> LoggedIn: { ViewData["LoggedInEmail"]}\n\t");
                    }
                }
                else
                {
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();

                }
                return View();
            }
            return Unauthorized("You must be logged into an authorized account to view this page!");

        }

        //[HttpPost("create"), ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreatePerson(NewPersonVM personVM)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        //return error messages
        //        return View(personVM);
        //    }


        //    var response = await _personService.CreatePersonAsync(personVM);
        //    if (response.Status == HttpStatusCode.Unauthorized)
        //    {
        //        //if response is 401, it means access token has expired
        //        return RedirectToAction("refresh", "auth", new { returnUrl = "/people/createPerson" });
        //    }
        //    if (response.Status != HttpStatusCode.OK)
        //    {
        //        ModelState.AddModelError(string.Empty, "An Error Occurred while processing this request. Please try again in some time.");
        //    }
        //    return Ok("Person Created");
        //}

        //TODO
        //TODO
        //TODO
        //TODO
        //TODO
        //TODO
        
        [Route("u/{input}/edit/{edit}")]
        [HttpGet]
        public async Task<IActionResult> UpdatePerson(string input, string edit)
        {
            await Console.Out.WriteLineAsync($"\n\n\n\n\n\n\n--GET---UpdatePerson()----ACCESSING------------\n");
            await Console.Out.WriteLineAsync($"\n\n--GET---UpdatePerson()----Checking tmpData---\n\n\t{TempData["tEmail"]}");
            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(input);
            await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(id);
            //await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(input);//, criteria);
            await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)------userPersonExists?-----\n\t{userPersonExists}");

            var url = Url.RouteUrl("DexListDestination");
            // if the target exists...
            //if (userExists)
            if (userPersonExists)
            {
                if (userFlag && currentUserFlag)
                {
                    //await _userSessionService.SetTempPersonAsync(criteria);

                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail();
                    //return RedirectToAction("DetailsViewByRoute", "People");


                    // - get all person by user
                        // - get dexholdervm -> .People.include fullname
                    // - get the person by searching criteria
                    var personPlus = await _personService.GetPersonDexListVMAsync(input, edit);
                    EditPersonFullVM? editPersonVM = null;

                    // - if person is not null
                    if (personPlus != null)
                    {
                        // - instantiate new vm
                        editPersonVM = personPlus.getEditInstance();
                    }
                    // - endif
                    
                    if (editPersonVM != null)
                    {
                        //populate ViewData here


                        ViewData["_LocalCounter"] = personPlus.LocalCounter;
                        //ViewData["_Email"] = personPlus.AppEmail;
                        ViewData["_Id"] = editPersonVM.Id;
                        ViewData["_Nickname"] = editPersonVM.Nickname;
                        ViewData["_NameFirst"] = editPersonVM.NameFirst;
                        ViewData["_NameMiddle"] = editPersonVM.NameMiddle;
                        ViewData["_NameLast"] = editPersonVM.NameLast;
                        ViewData["_PhNameFirst"] = editPersonVM.PhNameFirst;
                        ViewData["_PhNameMiddle"] = editPersonVM.PhNameMiddle;
                        ViewData["_PhNameLast"] = editPersonVM.PhNameLast;
                        ViewData["_DateOfBirth"] = editPersonVM.DateOfBirth;
                        ViewData["_Gender"] = editPersonVM.Gender;
                        ViewData["_Pronouns"] = editPersonVM.Pronouns;
                        ViewData["_Rating"] = editPersonVM.Rating;
                        ViewData["_Favorite"] = editPersonVM.Favorite;

                        TempData["_LocalCounter"] = personPlus.LocalCounter;
                        //TempData["_Email"] = personPlus.AppEmail;
                        TempData["_Id"] = editPersonVM.Id;
                        TempData["_Nickname"] = editPersonVM.Nickname;
                        TempData["_NameFirst"] = editPersonVM.NameFirst;
                        TempData["_NameMiddle"] = editPersonVM.NameMiddle;
                        TempData["_NameLast"] = editPersonVM.NameLast;
                        TempData["_PhNameFirst"] = editPersonVM.PhNameFirst;
                        TempData["_PhNameMiddle"] = editPersonVM.PhNameMiddle;
                        TempData["_PhNameLast"] = editPersonVM.PhNameLast;
                        TempData["_DateOfBirth"] = editPersonVM.DateOfBirth;
                        TempData["_Gender"] = editPersonVM.Gender;
                        TempData["_Pronouns"] = editPersonVM.Pronouns;
                        TempData["_Rating"] = editPersonVM.Rating;
                        TempData["_Favorite"] = editPersonVM.Favorite;






                        ViewData["EditPersonData"] = editPersonVM;
                        return View(editPersonVM);
                    }
                    // - pass the resulting EditFullPersonVM into view
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    //await _userSessionService.SetTempPersonAsync(criteria);
                    await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();



                    var personPlus = await _personService.GetPersonDexListVMAsync(input, edit);
                    await Console.Out.WriteLineAsync($"\n--GET---DetailsViewByRoute(id)------ {personPlus?.Nickname??"Response is null!"} -----\n\t");
                    //await Console.Out.WriteLineAsync($"\n--GET---DetailsViewByRoute(id)------ {personPlus?.Nickname??"Response is null!"} -----\n\tSTATUS:\t{personPlusResponse?.Status.ToString()??"Status is null!"}");
                    EditPersonFullVM? editPersonVM = null;

                    //PersonPlusDexListVM? personPlus = personPlusResponse.getPlusDexInstance();
                    //await Console.Out.WriteLineAsync($"\n--GET---DetailsViewByRoute(id)------ personPlusGetInstance: not null? {personPlus!=null} -----\n\tSTATUS:\t{personPlusResponse?.Status.ToString()??"Status is null!"}");

                    // - if person is not null
                    if (personPlus != null)
                    {
                        // - instantiate new vm
                        editPersonVM = personPlus.getEditInstance();
                    }
                    // - endif

                    if (editPersonVM != null)
                    {

                        ViewData["_testData"] = "this is a test";
                        TempData["_tmpData"] = "this is a test of tempData";

                        ViewData["_LocalCounter"] = personPlus.LocalCounter;
                        //ViewData["_Email"] = personPlus.AppEmail;

                        ViewData["_Email"] = editPersonVM.Email;
                        ViewData["_Id"] = editPersonVM.Id;
                        ViewData["_Nickname"] = editPersonVM.Nickname;
                        ViewData["_NameFirst"] = editPersonVM.NameFirst;
                        ViewData["_NameMiddle"] = editPersonVM.NameMiddle;
                        ViewData["_NameLast"] = editPersonVM.NameLast;
                        ViewData["_PhNameFirst"] = editPersonVM.PhNameFirst;
                        ViewData["_PhNameMiddle"] = editPersonVM.PhNameMiddle;
                        ViewData["_PhNameLast"] = editPersonVM.PhNameLast;
                        ViewData["_DateOfBirth"] = editPersonVM.DateOfBirth;
                        ViewData["_Gender"] = editPersonVM.Gender;
                        ViewData["_Pronouns"] = editPersonVM.Pronouns;
                        ViewData["_Rating"] = editPersonVM.Rating;
                        ViewData["_Favorite"] = editPersonVM.Favorite;



                        TempData["_tmpLocalCounter"] = personPlus.LocalCounter;
                        //TempData["_tmpEmail"] = personPlus.AppEmail;
                        TempData["_tmpEmail"] = editPersonVM.Email;
                        //TempData["_tmpId"] = editPersonVM.Id;
                        TempData["_tmpNickname"] = editPersonVM.Nickname;
                        TempData["_tmpNameFirst"] = editPersonVM.NameFirst;
                        TempData["_tmpNameMiddle"] = editPersonVM.NameMiddle;
                        TempData["_tmpNameLast"] = editPersonVM.NameLast;
                        TempData["_tmpPhNameFirst"] = editPersonVM.PhNameFirst;
                        TempData["_tmpPhNameMiddle"] = editPersonVM.PhNameMiddle;
                        TempData["_tmpPhNameLast"] = editPersonVM.PhNameLast;
                        TempData["_tmpDateOfBirth"] = editPersonVM.DateOfBirth;
                        TempData["_tmpGender"] = editPersonVM.Gender;
                        TempData["_tmpPronouns"] = editPersonVM.Pronouns;
                        //TempData["_tmpRating"] = editPersonVM.Rating;
                        //TempData["_tmpFavorite"] = editPersonVM.Favorite;


                        ViewData["EditPersonData"] = editPersonVM;
                        //TempData["_tmpEditPersonData"] = editPersonVM;
                        return View(editPersonVM);
                    }


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


            //return View();
        }











        [Route("u/{id}/p/{criteria}")]
        [HttpGet(Name = "PersonDetails")]
        public async Task<IActionResult> DetailsViewByRoute(string id, string criteria)
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
                    await _userSessionService.SetTempPersonAsync(criteria);

                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail();
                    //return RedirectToAction("DetailsViewByRoute", "People");
                    
                    
                    
                    try
                    {
                        var person = await _personService.GetPersonPlusDexListVMAsync(id, criteria);
                        if (person == null)
                        {
                            return NotFound();
                        }
                        await _userSessionService.SetTempPersonAsync(person.RecordCollectorId, person.ContactInfoId);
                        TempData["tRecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        ViewData["RecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        TempData["tContactInfoId"] = _userSessionService.GetTempContactInfo();
                        ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();
                        
                        return View(); // assumes you have Views/People/DeletePerson.cshtml
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in DetailsPerson view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for viewing.");
                    }


                    
                    
                    
                    return View();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    await _userSessionService.SetTempPersonAsync(criteria);
                    await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();

                    // HACK disbling for now for if an admin goes to another user to create a person
                    //await _userSessionService.CloseTempSessionData();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                    
                    
                    
                    try
                    {
                        var person = await _personService.GetPersonPlusDexListVMAsync(id, criteria);
                        if (person == null)
                        {
                            return NotFound();
                        }
                        await _userSessionService.SetTempPersonAsync(person.RecordCollectorId, person.ContactInfoId);
                        TempData["tRecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        ViewData["RecordCollectorId"] = _userSessionService.GetTempRecordCollector();
                        TempData["tContactInfoId"] = _userSessionService.GetTempContactInfo();
                        ViewData["ContactInfoId"] = _userSessionService.GetTempContactInfo();
                        

                        return View(); // assumes you have Views/People/DeletePerson.cshtml
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in DetailsPerson view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for viewing.");
                    }
//TODO                                      

                    
                    
                    
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


            //return View();
        }





        [HttpGet("u/{input}/delete/{criteria}")]
        public async Task<IActionResult> DeletePerson(string input, string criteria)
        {

            await Console.Out.WriteLineAsync($"\n\n--GET---DeletePerson(input, criteria)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---DeletePerson(input, criteria)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---DeletePerson(input, criteria)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(input);
            await Console.Out.WriteLineAsync($"\n\n--GET---DeletePerson(input, criteria)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(input);
            //await Console.Out.WriteLineAsync($"\n\n--GET---DeletePerson(input, criteria)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(input);//, criteria);
            await Console.Out.WriteLineAsync($"\n\n--GET---DeletePerson(input, criteria)------userPersonExists?-----\n\t{userPersonExists}");

            var url = Url.RouteUrl("DexListDestination");
            // if the target exists...
            //if (userExists)
            if (userPersonExists)
            {
                if (userFlag && currentUserFlag)
                {
                    await _userSessionService.SetTempPersonAsync(criteria);

                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail();

                    try
                    {
                        var person = await _personService.GetPersonPlusDexListVMAsync(input, criteria);
                        if (person == null)
                        {
                            return NotFound();
                        }

                        return View("DeletePerson", person); // assumes you have Views/People/DeletePerson.cshtml
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in DeletePerson view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for deletion.");
                    }




                    //return RedirectToAction("DetailsViewByRoute", "People");
                    //return View();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    await _userSessionService.SetTempPersonAsync(criteria);
                    await Console.Out.WriteLineAsync($"\n\n--GET---DeletePerson(input, criteria)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    ViewData["LastPerson"] = _userSessionService.GetTempPerson();

                    try
                    {
                        var person = await _personService.GetPersonPlusDexListVMAsync(input, criteria);
                        if (person == null)
                        {
                            return NotFound();
                        }

                        return View("DeletePerson", person); // assumes you have Views/People/DeletePerson.cshtml
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in DeletePerson view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for deletion.");
                    }





                    // HACK disbling for now for if an admin goes to another user to create a person
                    //await _userSessionService.CloseTempSessionData();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                    //return View();
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



                        //ViewData["_ApplicationUserId"] = editUserVM.ApplicationUserId;
                        //ViewData["_ApplicationUserName"] = editUserVM.ApplicationUserName;
                        //ViewData["_ApplicationEmail"] = editUserVM.ApplicationEmail;
                        //ViewData["_FirstName"] = editUserVM.FirstName;
                        //ViewData["_MiddleName"] = editUserVM.MiddleName;
                        //ViewData["_LastName"] = editUserVM.LastName;
                        //ViewData["_Gender"] = editUserVM.Gender;
                        //ViewData["_Pronouns"] = editUserVM.Pronouns;
                        //ViewData["_DateOfBirth"] = editUserVM.DateOfBirth;
                        //ViewData["_DexId"] = editUserVM.DexId;


                        //TempData["_ApplicationUserId"] = editUserVM.ApplicationUserId;
                        //TempData["_ApplicationUserName"] = editUserVM.ApplicationUserName;
                        //TempData["_ApplicationEmail"] = editUserVM.ApplicationEmail;
                        //TempData["_FirstName"] = editUserVM.FirstName;
                        //TempData["_MiddleName"] = editUserVM.MiddleName;
                        //TempData["_LastName"] = editUserVM.LastName;
                        //TempData["_Gender"] = editUserVM.Gender;
                        //TempData["_Pronouns"] = editUserVM.Pronouns;
                        //TempData["_DateOfBirth"] = editUserVM.DateOfBirth;
                        //TempData["_DexId"] = editUserVM.DexId;




        [Route("edit/{input}")]
        [HttpGet]
        public async Task<IActionResult> UpdateDex(string input)  //, string edit)
        {
            await Console.Out.WriteLineAsync($"\n\n\n\n\n\n\n--GET---UpdateDex()----ACCESSING------------\n");
            await Console.Out.WriteLineAsync($"\n\n--GET---UpdateDex()----Checking tmpData---\n\n\t{TempData["tEmail"]}");
            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(input);
            await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(id);
            //await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(input);//, criteria);
            await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)------userPersonExists?-----\n\t{userPersonExists}");

            var url = Url.RouteUrl("DexListDestination");
            // if the target exists...
            //if (userExists)
            if (userPersonExists)
            {
                if (userFlag && currentUserFlag)
                {
                    //await _userSessionService.SetTempPersonAsync(criteria);

                    //TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    //ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail();
                    //return RedirectToAction("DetailsViewByRoute", "People");


                    // - get all person by user
                    // - get dexholdervm -> .People.include fullname
                    // - get the person by searching criteria
                    var dexVM = await _personService.GetDexHolderMiddleVMAsync(input);
                    DexHolderUserEditVM? editUserVM = null;

                    // - if person is not null
                    if (dexVM != null)
                    {
                        // - instantiate new vm
                        editUserVM = dexVM.getEditInstance();
                    }
                    // - endif

                    if (editUserVM != null)
                    {
                        //populate ViewData here


                        ViewData["_ApplicationUserId"] = editUserVM.ApplicationUserId;
                        ViewData["_ApplicationUserName"] = editUserVM.ApplicationUserName;
                        ViewData["_ApplicationEmail"] = editUserVM.ApplicationEmail;
                        ViewData["_FirstName"] = editUserVM.FirstName;
                        ViewData["_MiddleName"] = editUserVM.MiddleName;
                        ViewData["_LastName"] = editUserVM.LastName;
                        ViewData["_Gender"] = editUserVM.Gender;
                        ViewData["_Pronouns"] = editUserVM.Pronouns;
                        ViewData["_DateOfBirth"] = editUserVM.DateOfBirth;
                        ViewData["_DexId"] = editUserVM.DexId;


                        TempData["_ApplicationUserId"] = editUserVM.ApplicationUserId;
                        TempData["_ApplicationUserName"] = editUserVM.ApplicationUserName;
                        TempData["_ApplicationEmail"] = editUserVM.ApplicationEmail;
                        TempData["_FirstName"] = editUserVM.FirstName;
                        TempData["_MiddleName"] = editUserVM.MiddleName;
                        TempData["_LastName"] = editUserVM.LastName;
                        TempData["_Gender"] = editUserVM.Gender;
                        TempData["_Pronouns"] = editUserVM.Pronouns;
                        TempData["_DateOfBirth"] = editUserVM.DateOfBirth;
                        TempData["_DexId"] = editUserVM.DexId;



                        return View(editUserVM);
                    }
                    // - pass the resulting EditFullPersonVM into view
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    //await _userSessionService.SetTempPersonAsync(criteria);
                    await Console.Out.WriteLineAsync($"\n\n--GET---DetailsViewByRoute(id)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();
                    //TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    //ViewData["LastPerson"] = _userSessionService.GetTempPerson();



                    var dexVM = await _personService.GetDexHolderMiddleVMAsync(input);//, edit);
                    await Console.Out.WriteLineAsync($"\n--GET---DetailsViewByRoute(id)------ {dexVM?.ApplicationUserName ?? "Response is null!"} -----\n\t");
                    //await Console.Out.WriteLineAsync($"\n--GET---DetailsViewByRoute(id)------ {dexVM?.Nickname??"Response is null!"} -----\n\tSTATUS:\t{dexVMResponse?.Status.ToString()??"Status is null!"}");
                    DexHolderUserEditVM? editUserVM = null;

                   // PersonPlusDexListVM? dexVM = dexVMResponse.getPlusDexInstance();
                    //await Console.Out.WriteLineAsync($"\n--GET---DetailsViewByRoute(id)------ dexVMGetInstance: not null? {dexVM!=null} -----\n\tSTATUS:\t{dexVMResponse?.Status.ToString()??"Status is null!"}");

                    // - if person is not null
                    if (dexVM != null)
                    {
                        // - instantiate new vm
                        editUserVM = dexVM.getEditInstance();
                    }
                    // - endif

                    if (editUserVM != null)
                    {

                        ViewData["_ApplicationUserId"] = editUserVM.ApplicationUserId;
                        ViewData["_ApplicationUserName"] = editUserVM.ApplicationUserName;
                        ViewData["_ApplicationEmail"] = editUserVM.ApplicationEmail;
                        ViewData["_FirstName"] = editUserVM.FirstName;
                        ViewData["_MiddleName"] = editUserVM.MiddleName;
                        ViewData["_LastName"] = editUserVM.LastName;
                        ViewData["_Gender"] = editUserVM.Gender;
                        ViewData["_Pronouns"] = editUserVM.Pronouns;
                        ViewData["_DateOfBirth"] = editUserVM.DateOfBirth;
                        ViewData["_DexId"] = editUserVM.DexId;


                        TempData["_ApplicationUserId"] = editUserVM.ApplicationUserId;
                        TempData["_ApplicationUserName"] = editUserVM.ApplicationUserName;
                        TempData["_ApplicationEmail"] = editUserVM.ApplicationEmail;
                        TempData["_FirstName"] = editUserVM.FirstName;
                        TempData["_MiddleName"] = editUserVM.MiddleName;
                        TempData["_LastName"] = editUserVM.LastName;
                        TempData["_Gender"] = editUserVM.Gender;
                        TempData["_Pronouns"] = editUserVM.Pronouns;
                        TempData["_DateOfBirth"] = editUserVM.DateOfBirth;
                        TempData["_DexId"] = editUserVM.DexId;

                        //TempData["_tmpEditPersonData"] = editUserVM;
                        return View(editUserVM);
                    }


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


            //return View();
        }

        /**-------------------------------------------------------------*/

        [HttpGet("delete/{input}")]
        public async Task<IActionResult> DeleteUser(string input)
        {

            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteUser(input, criteria)----ACCESSING---------\n");

            if (!_userSessionService.IsLoggedIn()) RedirectToAction("Login", "Auth");

            bool roleFlag = await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator");
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteUser(input, criteria)-----------roleFlag---\n\t{roleFlag}");
            bool userFlag = await _userSessionService.HasAnyRoleAsync("User");
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteUser(input, criteria)-----------userFlag---\n\t{userFlag}");
            bool currentUserFlag = await _authService.CheckInputAgainstSessionAsync(input);
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteUser(input, criteria)----currentUserFlag---\n\t{currentUserFlag}");

            //bool userExists = await _authService.CheckIfUserExistsAsync(input);
            //await Console.Out.WriteLineAsync($"\n\n--GET---DeleteUser(input, criteria)------userExists?-----\n\t{userExists}");
            bool userPersonExists = await _authService.CheckIfUserExistsAsync(input);//, criteria);
            await Console.Out.WriteLineAsync($"\n\n--GET---DeleteUser(input, criteria)------userPersonExists?-----\n\t{userPersonExists}");

            var url = Url.RouteUrl("DexListDestination");
            // if the target exists...
            //if (userExists)
            if (userPersonExists)
            {
                if (userFlag && currentUserFlag)
                {
                    //await _userSessionService.SetTempPersonAsync(criteria);

                    //TempData["LastPerson"] = _userSessionService.GetTempPerson();
                    //ViewData["LastPerson"] = _userSessionService.GetTempPerson();
                    // we're working with the confirmed current user, so just get the email that's stored in user session service
                    ViewData["LoggedInEmail"] = _userSessionService.GetEmail();
                    TempData["tEmail"] = _userSessionService.GetEmail();

                    try
                    {
                        var person = await _personService.GetDexHolderMiddleVMAsync(input);
                        if (person == null)
                        {
                            return NotFound();
                        }

                        return View("DeleteUser", person); // assumes you have Views/People/DeleteUser.cshtml
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in DeleteUser view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for deletion.");
                    }




                    //return RedirectToAction("DetailsViewByRoute", "People");
                    //return View();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                }
                else if (roleFlag)
                {

                    await Console.Out.WriteLineAsync($"\n\n--GET---DeleteUser(input, criteria)------Using temp email!-----\n\t{_userSessionService.GetTempEmail()}");
                    ViewData["LoggedInEmail"] = _userSessionService.GetTempEmail();
                    TempData["tEmail"] = _userSessionService.GetTempEmail();

                    try
                    {
                        var person = await _personService.GetDexHolderMiddleVMAsync(input);
                        if (person == null)
                        {
                            return NotFound();
                        }

                        return View("DeleteUser", person); // assumes you have Views/People/DeleteUser.cshtml
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error in DeleteUser view endpoint: " + ex.Message);
                        return BadRequest("Error fetching person for deletion.");
                    }





                    // HACK disbling for now for if an admin goes to another user to create a person
                    //await _userSessionService.CloseTempSessionData();
                    //return ControllerContext.MyDisplayRouteInfo("", $" URL = {url}");
                    //return View();
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





    }


}
