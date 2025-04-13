using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Settings;
using NetDexTest_01.Constants;
using NetDexTest_01.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace NetDexTest_01.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly IUserRepository _userRepo;


        public UserService(IUserRepository userRepo, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _userRepo = userRepo;

        }

        /// <summary>
        /// Register (Create) a new User, but only if they do not exist already (based on both the username and email)
        /// </summary>
        /// <remarks>For a method that returns an object, use <see cref="CreateRegisterAsync(RegisterModel)"/></remarks>
        /// <param name="model">A model object that contains fields for both <see cref="ApplicationUser">ApplicationUser</see> and <see cref="DexHolder">DexHolder</see></param>
        /// <returns>String message to confirm if a user was registered or already exists</returns>
        public async Task<string> RegisterAsync(RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
            };
            var tempDexHolder = new DexHolder
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Gender = model.Gender,
                Pronouns = model.Pronouns
            };



            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            var userWithSameUsername = await _userManager.FindByNameAsync(model.Username);//EmailAsync(model.Email);

            if (userWithSameEmail == null && userWithSameUsername == null)
            {

                //var result = await _userManager.CreateAsync(user, model.Password);

                var result = await _userRepo.CreateUserDexHolderAsync(user, tempDexHolder, model.Password);
                if (result != null) //result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Authorization.default_role.ToString());

                }
                return $"User Registered with username {user.UserName}";
            }
            else
            {
                return $"Email {user.Email} is already registered.";
            }


        }




        /// <remarks>For a method that returns an string confirmation, use <see cref="RegisterAsync(RegisterModel)"/></remarks>
        /// <param name="model">A model object that contains fields for both <see cref="ApplicationUser">ApplicationUser</see> and <see cref="DexHolder">DexHolder</see></param>
        /// <returns><see cref="ApplicationUser" /> or null, based on if a User already exists in the database</returns>
        /// <inheritdoc cref="RegisterAsync(RegisterModel)"/>
        public async Task<ApplicationUser?> CreateRegisterAsync(RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
            };
            var tempDexHolder = new DexHolder
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Gender = model.Gender,
                Pronouns = model.Pronouns
            };



            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            var userWithSameUsername = await _userManager.FindByNameAsync(model.Username);//EmailAsync(model.Email);

            if (userWithSameEmail == null && userWithSameUsername == null)
            {

                //var result = await _userManager.CreateAsync(user, model.Password);

                var result = await _userRepo.CreateUserDexHolderAsync(user, tempDexHolder, model.Password);
                if (result != null) //result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Authorization.default_role.ToString());

                }
                return result ;//$"User Registered with username {user.UserName}";
            }
            else
            {
                return null ;//$"Email {user.Email} is already registered.";
            }


        }





        public async Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model)
        {
            var authenticationModel = new AuthenticationModel();    // Create a new Response Object
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)       // Check if passed email is valid. Return a message if not valid
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $"No Accounts Registered with {model.Email}.";
                return authenticationModel;
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password))        // Checks if the password is valid, else return a message saying incorrect credentials
            {
                authenticationModel.IsAuthenticated = true;
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);     // Call CreateJWToken function
                authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authenticationModel.Email = user.Email;
                authenticationModel.UserName = user.UserName;
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                authenticationModel.Roles = rolesList.ToList();
                return authenticationModel;                                         // returns the response object
            }
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
            return authenticationModel;
        }
        /// <summary>
        /// Build the JWToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user) // build the JWToken
        {
            var userClaims = await _userManager.GetClaimsAsync(user);   // gets all claims of the user ( user details )
            var roles = await _userManager.GetRolesAsync(user);         // gets all the roles of the user

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim("uid", user.Id)
    }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(        // creates a new JWT Security Token 
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;                // returns the new JWT Security Token 
        }



        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return $"No Accounts Registered with {model.Email}.";
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password))    // core function if the user is a valid one
            {
                var roleExists = Enum.GetNames(typeof(Authorization.Roles)).Any(x => x.ToLower() == model.Role.ToLower()); // check if the passed Role is present in our system. If not, throw an error message
                if (roleExists)
                {
                    var validRole = Enum.GetValues(typeof(Authorization.Roles)).Cast<Authorization.Roles>().Where(x => x.ToString().ToLower() == model.Role.ToLower()).FirstOrDefault();
                    await _userManager.AddToRoleAsync(user, validRole.ToString());       // adds role to valid user
                    return $"Added {model.Role} to user {model.Email}.";    
                }
                return $"Role {model.Role} not found.";
            }
            return $"Incorrect Credentials for user {user.Email}.";
        }

    }
}
