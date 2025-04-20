using Microsoft.AspNetCore.Mvc;
using System.Net;
using NetDexTest_01_MVC.Models;
using NetDexTest_01_MVC.Models.Entities;
using NetDexTest_01_MVC.Models.Authentication;

namespace NetDexTest_01_MVC.Controllers
{
    public class PeopleController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}


        [HttpGet]
        public async Task<IActionResult> CreatePerson()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePerson(Person person)
        {
            if (!ModelState.IsValid)
            {
                //return error messages
                return View(person);
            }

            var response = await _personService.CreatePersonAsync(person);
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
