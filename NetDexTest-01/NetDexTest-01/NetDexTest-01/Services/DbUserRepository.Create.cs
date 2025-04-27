using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models.ViewModels;
using NetDexTest_01.Models;
//using NetDexTest_01.Models.;
using NetDexTest_01.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

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
                await _userManager.AddToRoleAsync(user, "User");


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
                await _userManager.AddToRoleAsync(user, "User");


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
            var roleresult = await _userManager.AddToRoleAsync(user, "User");
            if (roleresult == IdentityResult.Success)
            {
                _logger.LogInformation("\n\nUser given a role!.\n\n");
            }
            else
            {
                _logger.LogInformation("\n\nUser unable to be given a role!!!.\n\n");
            }

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
            var roleresult = await _userManager.AddToRoleAsync(user, "User");
            if (roleresult == IdentityResult.Success)
            {
                _logger.LogInformation("\n\nUser given a role!.\n\n");
            }
            else
            {
                _logger.LogInformation("\n\nUser unable to be given a role!!!.\n\n");
            }

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
                var roleresult = await _userManager.AddToRoleAsync(user, "User");
                if(roleresult == IdentityResult.Success)
                {
                    _logger.LogInformation("\n\nUser given a role!.\n\n");
                }
                else
                {
                    _logger.LogInformation("\n\nUser unable to be given a role!!!.\n\n");
                }

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

                    await _userManager.AddToRoleAsync(user, "User");
                    await _userManager.AddToRoleAsync(user, "BatchCreated");
                    
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







        public async Task<AuthenticatedResponse> GetTokens(ApplicationUser user)
        {
            await Console.Out.WriteAsync($"\n\n\n\n----------Get Tokens------------\n\n\n");
            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["JwtSettings:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email)
                        //new Claim("DexHolderId", user.DexHolder.Id)
                    };
            await Console.Out.WriteAsync($"\nclaims:\n\t\t{claims}\n\n");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            await Console.Out.WriteAsync($"\nkey:\n\t\t{key}\n\n");
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            await Console.Out.WriteAsync($"\nsignIn:\n\t\t{signIn}\n\n");
            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:MinutesToExpiration"])),
                signingCredentials: signIn);

            await Console.Out.WriteAsync($"\ntoken:\n\t\t{token}\n\n");

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            await Console.Out.WriteAsync($"\ntokenStr:\n\t\t{tokenStr}\n\n");

            var refreshTokenStr = GetRefreshToken();
            await Console.Out.WriteAsync($"\nrefreshTokenStr:\n\t\t{refreshTokenStr}\n\n");
            user.RefreshToken = refreshTokenStr;
            var authResponse = new AuthResponse { Token = tokenStr, RefreshToken = refreshTokenStr, UserOut = user.Email };
            await Console.Out.WriteAsync($"\nauthResponse:\n\t\t{authResponse}\n\n");

            await Console.Out.WriteAsync($"\n\n\n\n-----END--Get Tokens------------\n\n\n");

            return await Task.FromResult(authResponse);
        }
        /*------------------------*/
        public string GetRefreshToken()
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            // ensure token is unique by checking against db
            var tokenIsUnique = !_userManager.Users.Any(u => u.RefreshToken == token);
            //var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == request.RefreshToken);



            if (!tokenIsUnique)
                return GetRefreshToken();  //recursive call

            return token;
        }
        public async Task<ApplicationUser> GetUserByRefreshToken(string refreshToken)
        {
            return await Task.FromResult(_userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken));
            //return await Task.FromResult(tempUserDb.FirstOrDefault(u => u.RefreshToken == refreshToken));
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])),
                ValidateLifetime = false //we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }











    }
}
