using Microsoft.AspNetCore.Mvc;
using System.Net;
using NetDexTest_01_MVC.Models;
using NetDexTest_01_MVC.Models.ViewModels;
using NetDexTest_01_MVC.Models.Entities;
using NetDexTest_01_MVC.Models.Authentication;
using NetDexTest_01_MVC.Services;

namespace NetDexTest_01_MVC.Controllers
{
    public class PeopleController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly IPersonService _personService;
        public PeopleController(IPersonService personService)
        {
            _personService = personService;
        }



        [HttpGet]
        public async Task<IActionResult> CreatePerson()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
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
