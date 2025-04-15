using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace NetDexTest_01.Services
{
    /// <summary>
    /// Choose a category that matches the property field. Used to cut down on redundant methods that use the same Type variables for different properties.
    /// <br />
    /// <list type="bullet">
    /// <item>
    /// <term>id</term>
    /// <description>String StringLength(450) </description>
    /// </item>
    /// <item>
    ///<term>username</term>
    /// <description>String StringLength(256) </description>
    /// </item>
    /// </list>
    /// <br />
    /// <remarks>Example - ApplicationUserId and ApplicationUsername are both strings</remarks>
    /// 
    /// </summary>
    public enum PropertyField
    {
        id,
        username,
        email
    }

    /// <summary>
    /// The user repository class that interacts with the Db.
    /// </summary>
    public partial class DbUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db; //_context
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        //private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<ApplicationUser> _logger;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbUserRepository"/> class.
        /// </summary>
        /// <param name="db">The db.</param>
        public DbUserRepository(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            ILogger<ApplicationUser> logger
            )
        {
            _db = db; //context
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            //_emailStore = GetEmailStore();
            _emailSender = emailSender;
            _signInManager = signInManager;
            _logger = logger;



        }


        public async Task<ICollection<ApplicationUser>> ReadAllApplicationUsers()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<ICollection<DexHolder>> ReadAllDexHolders()
        {
            return await _db.DexHolder.ToListAsync();
        }
        public async Task<ICollection<DexHolder>> ReadAllDexAsync()
        {
            return await _db.DexHolder.ToListAsync(); // I/O Bound Operation
        }



        public async Task<ApplicationUser?> GetByUsernameAsync(string username)
        {
            var conversion = await _db.Users.FirstOrDefaultAsync(u => u.UserName == username);
            return (ApplicationUser?)conversion;
        }
        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            var conversion = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            return (ApplicationUser?)conversion;
        }

        public async Task<ApplicationUser?> GetUserAsync(PropertyField pType, string input)
        {
            ApplicationUser? user = null;
            switch (pType)
            {
                case PropertyField.id:
                    user = await GetByIdAsync(input);
                    break;
                case PropertyField.username:
                    user = await GetByUsernameAsync(input);
                    break;
                default:
                    throw new InvalidOperationException("PropertyField is invalid! Check the switch-case and try again");
            }
            return user;
        }



        public async Task<ApplicationUser?> ReadByUsernameAsync(string username)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user != null)
            {
                user.Roles = await _userManager.GetRolesAsync(user);
            }
            return user;
        }
        public async Task<ApplicationUser?> ReadByIdAsync(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                user.Roles = await _userManager.GetRolesAsync(user);
            }
            return user;
        }
        public async Task<ApplicationUser?> ReadUserAsync(PropertyField pType, string input)
        {
            ApplicationUser? user = null;
            switch (pType)
            {
                case PropertyField.id:
                    user = await ReadByIdAsync(input);
                    break;
                case PropertyField.username:
                    user = await ReadByUsernameAsync(input);
                    break;
                default:
                    throw new InvalidOperationException("PropertyField is invalid! Check the switch-case and try again");
            }
            return user;
        }


        #region ReadDex
        public async Task<DexHolder?> ReadDexAsync(PropertyField pType, string input)
        {
            switch (pType)
            {
                case PropertyField.id:
                    return await ReadDexByIdAsync(input);
                    break;
                case PropertyField.username:
                    return await ReadDexByUsernameAsync(input);
                    break;
                default:
                    throw new InvalidOperationException("PropertyField is invalid! Check the switch-case and try again");
            }
            //return await _db.People.FindAsync(id);
            //return await _db.DexHolder.FirstOrDefaultAsync(p => p.ApplicationUserId == input); //Takes a lamda expression as its parameter. Slightly slower than first return option but slightly more flexible
        }
        public async Task<DexHolder?> ReadDexByIdAsync(string applicationUserId)
        {
            //return await _db.People.FindAsync(id);
            return await _db.DexHolder.FirstOrDefaultAsync(p => p.ApplicationUserId == applicationUserId); //Takes a lamda expression as its parameter. Slightly slower than first return option but slightly more flexible
        }
        public async Task<DexHolder?> ReadDexByUsernameAsync(string username)
        {
            //return await _db.People.FindAsync(id);
            return await _db.DexHolder.FirstOrDefaultAsync(p => p.ApplicationUserName == username); //Takes a lamda expression as its parameter. Slightly slower than first return option but slightly more flexible
        }
        #endregion ReadDex











        public async Task AssignUserToRoleAsync(string userName, string roleName)
        {
            var roleCheck = await _roleManager.RoleExistsAsync(roleName);
            if (!roleCheck)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            var user = await ReadByUsernameAsync(userName);
            if (user != null)
            {
                if (!user.HasRole(roleName))
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

        public DexHolder CreateTempDexHolder(string? firstName = null, string? middleName = null, string? lastName = null, string? gender = null, string? pronouns = null)
        {
            var dexHolder = new DexHolder
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Gender = gender,
                Pronouns = pronouns
            };
            return dexHolder;
        }



        #region CreateIndividual
        public async Task<ApplicationUser> CreateAsync(
            ApplicationUser user, string password)
        {
            await _userManager.CreateAsync(user, password);
            return user;
        }


        public async Task<DexHolder> CreateDexAsync(DexHolder dexHolder)
        {
            // Add person to the database
            await _db.DexHolder.AddAsync(dexHolder);
            // Commit the changes
            await _db.SaveChangesAsync();
            return dexHolder;
        }
        #endregion CreateIndividual

        #region CreateBoth

        // Empty method just for doccumentation purposes
        public async Task<ApplicationUser> CreateUserDexHolderAsync()
        {
            throw new NotImplementedException();
        }





        public async Task<ApplicationUser> CreateUserDexHolderAsync(
            ApplicationUser user, string password)
        {
            await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
            //cannot access _emailstore

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                // CHATGPT - Create corresponding DexHolder record
                var dexHolder = new DexHolder
                {
                    ApplicationUserId = user.Id,
                    ApplicationUserName = user.UserName,
                    FirstName = null,
                    MiddleName = null,
                    LastName = null,
                    Gender = null,
                    Pronouns = null
                };

                await AddDexHolderAsync(dexHolder);
                await SaveChangesAsync();


                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


            }
            return user;
        }

        public async Task<ApplicationUser> CreateUserDexHolderAsync(
    string email, string username, string password)
        {
            var user = new ApplicationUser
            {
                Email = email,
                UserName = username
            };

            await _userManager.CreateAsync(user, password);

            await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
            //cannot access _emailstore

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                // CHATGPT - Create corresponding DexHolder record
                var dexHolder = new DexHolder
                {
                    ApplicationUserId = user.Id,
                    ApplicationUserName = user.UserName,
                    FirstName = null,
                    MiddleName = null,
                    LastName = null,
                    Gender = null,
                    Pronouns = null
                };

                await AddDexHolderAsync(dexHolder);
                await SaveChangesAsync();


                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


            }
            return user;

        }

        public async Task<ApplicationUser> CreateUserDexHolderAsync(
            string userAndEmail, string password)
        {
            var user = new ApplicationUser { UserName = userAndEmail, Email = userAndEmail };
            await _userManager.CreateAsync(user, password);

            await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
            //cannot access _emailstore

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                // CHATGPT - Create corresponding DexHolder record
                var dexHolder = new DexHolder
                {
                    ApplicationUserId = user.Id,
                    ApplicationUserName = user.UserName,
                    FirstName = null,
                    MiddleName = null,
                    LastName = null,
                    Gender = null,
                    Pronouns = null
                };

                await AddDexHolderAsync(dexHolder);
                await SaveChangesAsync();


                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


            }
            return user;

        }

        // NOTE: RECCOMMENDED METHOD FOR CREATING USERS
        public async Task<ApplicationUser> CreateUserDexHolderAsync(
            ApplicationUser user, DexHolder tempDexHolder, string password)
        {
            //await _userManager.CreateAsync(user, password);

            //cannot access _emailstore

            await _userManager.CreateAsync(user, password);
            //if (result.Succeeded)
            //{
                //await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
                _logger.LogInformation("User created a new account with password.");

                // CHATGPT - Create corresponding DexHolder record
                var userId = await _userManager.GetUserIdAsync(user);
                var userName = await _userManager.GetUserNameAsync(user);
                    
                tempDexHolder!.ApplicationUserId = userId;
                tempDexHolder!.ApplicationUserName = userName;

                await AddDexHolderAsync(tempDexHolder);
                await SaveChangesAsync();

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //}
            return user;
        }

        #endregion CreateBoth


        #region CreateBothBatch
        public async Task<IEnumerable<ApplicationUser>> CreateUserDexHolderAsync(
            List<ApplicationUser> users, List<DexHolder> tempDexHolders, List<String> passwords)
        {
            //int[] nums = { 1, 2, 3, 4 };
            //string[] words = { "one", "two", "three", "four" };
            //string[] roman = { "I", "II", "III", "IV" };

            // Create a list to return
            var returnedUsers = new List<ApplicationUser>();


            // Zip allows iteration of multiple arrays at once. If arrays are different lengths, it will evaluate to the length of the shortest array
            // source: https://stackoverflow.com/a/41534351
            ApplicationUser[] uArray = users.ToArray();
            DexHolder[] dhArray = tempDexHolders.ToArray();
            string[] pArray = passwords.ToArray();

            foreach (var (user, dex, pass) in uArray.Zip(dhArray, pArray))
            {
                //Console.WriteLine($"{x}: {y} ({z})");

                // Add the user and password via UserManager, to generate Key values
                await _userManager.CreateAsync(user, pass);
                _logger.LogInformation("User created a new account with password.");

                // CHATGPT - Create corresponding DexHolder record
                var userId = await _userManager.GetUserIdAsync(user);
                var userName = await _userManager.GetUserNameAsync(user);

                // Add values of the user keys to the temporary DexHolder object
                dex!.ApplicationUserId = userId;
                dex!.ApplicationUserName = userName;

                // 
                try
                {

                    await AddDexHolderAsync(dex); // in DbUserRepository
                    await SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var logger = _logger;
                    logger.LogError(
                         $"\n\n---------------- CreateUserDexHolder (batch) --- log ---------\n\n"
                        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
                        //    Console.WriteLine(
                        + $"\n\n----- 1 -------- CreateUserDexHolder (batch) --- console ----\n\n"
                        //        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                        + $"{ex}"
                        + $"\n\n---- end ------- CreateUserDexHolder (batch) --- console ----\n\n");
                }


                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            
                returnedUsers.Add(user);
            }







            //}
            return returnedUsers;
        }


        #endregion CreateBothBatch


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="input"></param>
        /// <param name="dexHolder"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task UpdateDexAsync(PropertyField pType, string input, DexHolder dexHolder)
        {
            switch (pType)
            {
                case PropertyField.id:
                    await UpdateDexByIdAsync(input, dexHolder);
                    break;
                case PropertyField.username:
                    await UpdateDexByUsernameAsync(input, dexHolder);
                    break;
                default:
                    throw new InvalidOperationException("PropertyField is invalid! Check the switch-case and try again");
            }
        }


        public async Task UpdateDexByIdAsync(string id, DexHolder updatedDexHolder)
        {
            var dexHolderToUpdate = await ReadDexByIdAsync(id);
            dexHolderToUpdate!.FirstName = updatedDexHolder.FirstName; //Exclamation marks means we are guarenteeing it isn't null.
            dexHolderToUpdate!.LastName = updatedDexHolder.LastName;
            dexHolderToUpdate!.MiddleName = updatedDexHolder.MiddleName;
            dexHolderToUpdate!.DateOfBirth = updatedDexHolder.DateOfBirth;
            dexHolderToUpdate!.Gender  = updatedDexHolder.Gender;
            dexHolderToUpdate!.Pronouns  = updatedDexHolder.Pronouns;

            _db.SaveChanges();
        }

        public async Task UpdateDexByUsernameAsync(string username, DexHolder updatedDexHolder)
        {
            
            var dexHolderToUpdate = await ReadDexByUsernameAsync(username);
            dexHolderToUpdate!.FirstName = updatedDexHolder.FirstName; //Exclamation marks means we are guarenteeing it isn't null.
            dexHolderToUpdate!.LastName = updatedDexHolder.LastName;
            dexHolderToUpdate!.MiddleName = updatedDexHolder.MiddleName;
            dexHolderToUpdate!.DateOfBirth = updatedDexHolder.DateOfBirth;
            dexHolderToUpdate!.Gender = updatedDexHolder.Gender;
            dexHolderToUpdate!.Pronouns = updatedDexHolder.Pronouns;

            _db.SaveChanges();
        }


        public async Task UpdateUserWithDexAsync(string username, DexHolder dexHolder)
        {
            var userToUpdate = await ReadByUsernameAsync(username);
            userToUpdate!.DexHolder = dexHolder;
            //_userManager.ChangePasswordAsync;
            //_userManager.SetEmailAsync;
            //_userManager
            _db.SaveChanges();
        }


        /* TODO: Add UPDATE User information
         * 
         * Functions:
         *      - Update username, email, and/or password
         *          (password needs to be changed using UserManager)
         *      - Update corresponding DexHolder to maintain FK functionality
        */


        #region UpdateUser
        //public async verifyPassword

        //public async Task UpdateUserByUsername(PropertyField pType, string oldInput, ApplicationUser updatedUser)
        //{ }


        //public async Task UpdateUserByUsername(string oldUsername, string? oldPassword = null, string? newUsername = null, string? newPassword = null, ApplicationUser updatedUser)
        //{
        //    var userToUpdate = ReadUserAsync(PropertyField.username, oldUsername);
        //    var dexHolderToUpdate = ReadDexAsync(PropertyField.username, oldUsername);


        //    _userManager.CheckPasswordAsync

        //}
        #endregion UpdateUser

        public async Task DeleteAsync(PropertyField pType, string input)
        {
            switch (pType)
            {
                case PropertyField.id:
                    await DeleteByIdAsync(input);
                    break;
                case PropertyField.username:
                    await DeleteByUsernameAsync(input);
                    break;
                default:
                    throw new InvalidOperationException("PropertyField is invalid! Check the switch-case and try again");

            }
        }

            public async Task DeleteByIdAsync(string id)
        {
            var dexHolderToDelete = await ReadDexByIdAsync(id);
            var userToDelete = await ReadByIdAsync(id);
            if (dexHolderToDelete != null)
            {
                _db.DexHolder.Remove(dexHolderToDelete);
                await _db.SaveChangesAsync();
            }
            if (userToDelete != null)
            {
                _db.Users.Remove(userToDelete);
                await _db.SaveChangesAsync();

            }
        }
        public async Task DeleteByUsernameAsync(string username)
        {
            var dexHolderToDelete = await ReadDexByUsernameAsync(username);
            var userToDelete = await ReadByUsernameAsync(username);

            try
            {
                if (userToDelete != null)
                {
                    _db.Users.Remove(userToDelete);
                    await _db.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex}");
            }

            try
            {
                if (dexHolderToDelete != null)
                {
                    _db.DexHolder.Remove(dexHolderToDelete);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex}");
            }
        }


    }
} 

