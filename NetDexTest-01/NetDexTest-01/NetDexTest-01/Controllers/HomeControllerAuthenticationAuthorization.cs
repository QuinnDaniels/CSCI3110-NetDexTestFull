using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDexTest_01.Models;
using NetDexTest_01.Models.Entities;

using NetDexTest_01.Services;
using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text;



namespace NetDexTest_01.Controllers
{
    public partial class HomeController : Controller
    {

        public IActionResult GetUserName()
        {
            if (User.Identity!.IsAuthenticated)
            {
                string username = User.Identity.Name ?? "";
                return Content(username);
            }
            return Content("No user");
        }

        public async Task<IActionResult> GetUserId()
        {
            if (User.Identity!.IsAuthenticated)
            {
                string username = User.Identity.Name ?? "";
                var user = await _userRepo.ReadByUsernameAsync(username);
                if (user != null)
                {
                    return Content(user.Id);
                }
            }
            return Content("No user");
        }

    }
}
