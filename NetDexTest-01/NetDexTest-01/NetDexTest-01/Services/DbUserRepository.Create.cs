using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;

namespace NetDexTest_01.Services
{
    // Create Users
    public partial class DbUserRepository : IUserRepository
    {

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

        // moved from .GPT
        public async Task AddDexHolderAsync(DexHolder DexHolder)
        {
            await _db.DexHolder.AddAsync(DexHolder);
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



        public async Task<ApplicationUser> CreateUserDexHolderAsync(
    RegisterModel registerModel)
        {
            var user = new ApplicationUser { UserName = registerModel.Username, Email = registerModel.Email };


            await _userManager.CreateAsync(user, registerModel.Password);
            _logger.LogInformation("\n\nUser created a new account with password.\n\n");

            // CHATGPT - Create corresponding DexHolder record
            var userId = await _userManager.GetUserIdAsync(user);
            var userName = await _userManager.GetUserNameAsync(user);

            //if (userId != null && userName != null)
            //{
            var dexHolder = new DexHolder
            {
                ApplicationUserId = userId,
                ApplicationUserName = userName,
                FirstName = registerModel.FirstName,
                MiddleName = registerModel.MiddleName,
                LastName = registerModel.LastName,
                Gender = registerModel.Gender,
                Pronouns = registerModel.Pronouns
            };
            await AddDexHolderAsync(dexHolder);
            await SaveChangesAsync();
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            return user;
            //}
            //else
            //{
            //    throw new Exc
            //}
        }

        public async Task<IdentityResult> CreateUserDexHolderAsync(RegisterModel registerModel, Boolean inFlag)
        {
            var user = new ApplicationUser { UserName = registerModel.Username, Email = registerModel.Email };
            await Console.Out.WriteLineAsync($"\n\n\n CreateUserDexHolderAsync\n"
                                            + $"-------------------------------------\n"
                                            + $"\t\tBoolean \"inFlag\" detected!\tValue: {inFlag}\n"
                                            + $"\t\tMethod will return Task<IdentityResult> instead of Task<ApplicationUser>\n"
                                            + $"--------------------------------------\n\n\n");
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            _logger.LogInformation("\n\nUser created a new account with password.\n\n");

            // CHATGPT - Create corresponding DexHolder record
            var userId = await _userManager.GetUserIdAsync(user);
            var userName = await _userManager.GetUserNameAsync(user);

            if (userId != null && userName != null)
            {
                var dexHolder = new DexHolder
                {
                    ApplicationUserId = userId,
                    ApplicationUserName = userName,
                    FirstName = registerModel.FirstName,
                    MiddleName = registerModel.MiddleName,
                    LastName = registerModel.LastName,
                    Gender = registerModel.Gender,
                    Pronouns = registerModel.Pronouns
                };
                await AddDexHolderAsync(dexHolder);
                await SaveChangesAsync();
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            }
            return result;
            //else
            //{
            //    throw new Exc
            //}
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



    }
}
