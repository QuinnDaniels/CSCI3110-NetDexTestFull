using Microsoft.AspNetCore.Identity;
using NetDexTest_01.Models.Entities;
using System.Collections.Generic;
using NetDexTest_01.Contexts;
using NetDexTest_01.Constants;
using System.Net;


namespace NetDexTest_01.Services
{
    public class Initializer
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly NetDexTest_01.Constants.Authorization _authorization;

        public Initializer(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserRepository userRepository,
            NetDexTest_01.Constants.Authorization authorization
            )
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _authorization = authorization;
        }

        /// <summary>
        /// Seeds the users into the database upon creation.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task SeedUsersAsync()
        {
            // Ensure database is created
            _db.Database.EnsureCreated();


            #region seedRoles

            if (!_db.Roles.Any(r => r.Name == "Administrator"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Administrator" } );//( Authorization.Roles.Administrator.ToString() );
            }
            if (!_db.Roles.Any(r => r.Name == "Fake"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Fake" });
            }
            if (!_db.Roles.Any(r => r.Name == "Trainer"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Trainer" });
            }

            if (!_db.Roles.Any(r => r.Name == "Moderator"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Moderator" });
            }
            if (!_db.Roles.Any(r => r.Name == "User"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
            }

            #endregion seedRoles






            //if (!_db.Users.Any(u => u.UserName == "admin@test.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        Email = "admin@test.com",
            //        UserName = "admin@test.com",
            //    };
            //    await _userManager.CreateAsync(user, "Pass123!");
            //    await _userManager.AddToRoleAsync(user, "Admin");
            //}

            await SeedUserMethodAsync();
            await ApplicationDbContextSeed.SeedEssentialsAsync(_userManager, _roleManager); // From JWT Tutorial
        }

        /// <summary>
        /// Creates Users with DexHolder information to seed the database with
        /// </summary>
        /// <returns></returns>
        public async Task SeedUserMethodAsync()
        {
            var users = new List<ApplicationUser> { };
            var dexHolders = new List<DexHolder> { };
            List<String> passes = new List<string> { };

            ApplicationUser adminUser = new ApplicationUser
            {
                Email = "admin@test.com",
                UserName = "admin@test.com",
            };

            ApplicationUser fakeUser = new ApplicationUser
            {
                Email = "fake@email.com",
                UserName = "fake@email.com",
            };


            /*begin default users*/
            if (!_db.Users.Any(u => u.UserName == adminUser.UserName))
            {
                users.Add(adminUser);
                dexHolders.Add(new DexHolder { FirstName = null, LastName = null, Gender = null });
                passes.Add("AdminPass123!");
            }
            if (!_db.Users.Any(u => u.UserName == fakeUser.UserName))
            {
                users.Add(fakeUser);
                dexHolders.Add(new DexHolder { FirstName = "fake", LastName = "user", Gender = null });
                passes.Add("Pass123!");

            }
            /*end default users*/

            if (!_db.Users.Any(u => u.UserName == fakeUser.UserName))
            {
                users.Add(new ApplicationUser
                {
                    Email = "janedoe@email.com",
                    UserName = "janedoe@email.com",
                });
                dexHolders.Add(new DexHolder { FirstName = "Jane", LastName = "Doe", Gender = "woman" });
                passes.Add("Pass123!");

            }


            await _userRepository.CreateUserDexHolderAsync(users, dexHolders, passes);

            if (users.Contains(adminUser)) { await _userManager.AddToRoleAsync(adminUser, "Administrator"); }
            if (users.Contains(fakeUser)) { await _userManager.AddToRoleAsync(fakeUser, "Fake"); }


        }


        // TODO: Seed Persons 
       

    } 
}
        
