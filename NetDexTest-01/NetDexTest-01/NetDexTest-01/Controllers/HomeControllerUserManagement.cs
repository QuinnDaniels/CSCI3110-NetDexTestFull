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
        public async Task<IActionResult> CreateTestUser()
        {
            var n = _random.Next(100);
            var check = await _userRepo.ReadByUsernameAsync($"test{n}@test.com");
            if (check == null)
            {
                var user = new ApplicationUser
                {
                    Email = $"test{n}@test.com",
                    UserName = $"test{n}@test.com"
                    //FirstName = $"User{n}",
                    //LastName = $"Userlastname{n}"
                };
                await _userRepo.CreateAsync(user, "Pass123!");
                return Content($"Created test user 'test{n}@test.com' with password 'Pass123!'");
            }
            return Content("The user was already created.");
        }

        public async Task<IActionResult> TestAssignUserToRole()
        {
            await _userRepo.AssignUserToRoleAsync("fake@email.com", "TestRole");
            return Content("Assigned 'fake@email.com' to role 'TestRole'");
        }



        //public async Task<IActionResult> CreateTestDexHolder()
        //{
        //    var n = _random.Next(100);
        //    var check = await _userRepo.ReadByUsernameAsync($"dextest{n}@test.com");
        //    if (check == null)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            Email = $"test{n}@test.com",
        //            UserName = $"dextest{n}@test.com"
        //            //FirstName = $"User{n}",
        //            //LastName = $"Userlastname{n}"
        //        };


                

        //        await _userRepo.CreateAsync(user, "Pass123!");
        //        var checkTwo = await _userRepo.ReadByUsernameAsync(user.UserName);
        //        if (checkTwo == null)
        //        {
        //            var dex = new DexHolder
        //            {
        //                ApplicationUserName = user.UserName,
        //                ApplicationUser = user
        //                //FirstName = ,
        //                //MiddleName = ,
        //                //LastName = ,
        //                //DateOfBirth = ,
        //                //Gender = ,
        //                //Pronouns = 
        //            };

        //            await _userRepo.CreateDexAsync(dex);
        //            await _userRepo.UpdateUserWithDexAsync(user.UserName, dex);

        //        }


        //        return Content($"Created test user 'test{n}@test.com' with password 'Pass123!'");
        //    }
        //    return Content("The user was already created.");
        //}


        /// <summary>
        /// this one should work
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CreateTestUserDexHolder()
        {
            var n = _random.Next(100);
            var check = await _userRepo.ReadByUsernameAsync($"dextest{n}@test.com");
            if (check == null)
            {
                var user = new ApplicationUser();

                user!.Email = $"dextest{n}@test.com";
                user!.UserName = $"dextest{n}@test.com";
                //FirstName = $"User{n}",
                //LastName = $"Userlastname{n}"

                var tempDex = new DexHolder { FirstName = "Jane", LastName = "Doe", Gender = "Woman"};



                await _userRepo.CreateUserDexHolderAsync(user, tempDex, "Pass123!");

                var printerUser = await _userRepo.GetByUsernameAsync(user.UserName);


                return Content($"Created test user 'dextest{n}@test.com' with password 'Pass123!'\n\nUserId:\t{printerUser.Id}\nDexHolderId:\t{printerUser.DexHolder.Id}\nFirstName:\t{printerUser.DexHolder.FirstName}\nLastName:\t{printerUser.DexHolder.LastName}");
            }
            return Content("The user was already created.");
        }



        public async Task<IActionResult> DeleteFakeUser()
        {
            var check = await _userRepo.ReadByUsernameAsync("fake@email.com");
            string? tUserName = check.UserName;
            string? tEmail = check.Email;

            if (check != null)
            {
                await _userRepo.DeleteAsync(PropertyField.username, check.UserName);
                return Content($"Deleted User:\n{tUserName}\n{tEmail}");
            }
            return Content("The user was not found.");
        }

        public async Task<IActionResult> UpdateFakeUser()
        {
            var check = await _userRepo.ReadByUsernameAsync("fake@email.com");
            if (check != null)
            {

                var check2 = await _userRepo.ReadDexByUsernameAsync(check.UserName);
                if (check2 != null)
                {
                    string? tFirstName = check2.FirstName;
                    string? tMiddleName = check2.MiddleName;


                    string? tLastName = check2.LastName;

                    var oldCheck = check2;
                    check2!.FirstName = "John";
                    check2!.MiddleName = "F";
                    check2!.LastName = "Kennedy";

                    await _userRepo.UpdateDexByUsernameAsync(check2.ApplicationUserName, check2);
                    return Content($"{tFirstName}\t{tMiddleName}\t{tLastName}\n|\nV\n{check2.FirstName}\t{check2.MiddleName}\t{check2.LastName}");
                    

                }
                return Content($"DexHolder does not exist for fake user");
            }
            return Content("The user was not found.");
        }




        #region ROLES

        public async Task<IActionResult> ShowRoles(string userName)
        {
            if (userName != null)
            {
                ApplicationUser? user = await _userRepo.ReadByUsernameAsync(userName);
                StringBuilder builder = new();
                if (user != null)
                {
                    foreach (var roleName in user!.Roles)
                    {
                        builder.Append(roleName + " ");
                    }
                    return Content($"UserName: {user.UserName} Roles: {builder}");
                }
                else
                {
                    return Content("user is null!");
                }
            }
            return Content("username is null!");
        }


        public async Task<IActionResult> HasRole(string userName, string roleName)
        {
            string builder = "";

            if (userName != null)
            {
                ApplicationUser? user = await _userRepo.ReadByUsernameAsync(userName);
                if (user == null) { builder += $"\n------\nDB Error: Username {userName} not found!\n------\n"; }
                if (roleName == null) { builder += "\n------\nQuery Error: roleName cannot be null!\n------\n"; }
                if (user != null && roleName != null)
                {
                    if (user!.HasRole(roleName))
                    {
                        return Content($"{userName} has role {roleName}");
                    }
                    return Content($"{userName} does not have role {roleName}");
                }
                else { return Content($"{builder}"); }
            }
            else { return Content($"Username cannot be null!"); }

        }

        [Authorize(Roles = "TestRole")]
        public IActionResult TestRoleCheck()
        {
            return Content("Restricted to role TestRole");
        }

        #endregion ROLES



    }
}
