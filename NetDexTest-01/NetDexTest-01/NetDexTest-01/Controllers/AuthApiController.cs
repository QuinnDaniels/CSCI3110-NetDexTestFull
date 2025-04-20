using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NetDexTest_01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace NetDexTest_01.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        //public static List<User> tempUserDb = new List<User>{
        //    new User{UserId="abc123",UserName="John", DisplayName="BilboBaggins", Email="john@abc.com", Password="john@123" },
        //    new User{UserId="def456",UserName="Jane", DisplayName="Galadriel", Email="jane@xyz.com", Password="jane1995" }
        //};

        public IConfiguration _configuration;
        private UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepo;

        /*---------------*/
        public AuthController(IUserRepository userRepo, IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _configuration = config;
            _userManager = userManager;
            _userRepo = userRepo;
        }
        /*---------------*/
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestErrorMessages();
            }

            //var user = await _userRepo.GetUserAsync(PropertyField.email, request.Email, request.Password)
            //var user = await GetUser(request.Email, request.Password);
            var user = await _userManager.FindByEmailAsync(request.Email);
            var isAuthorized = user != null && await _userManager.CheckPasswordAsync(user, request.Password);

            //if (user != null)
            if (isAuthorized)
            {
                var authResponse = await GetTokens(user);
                user.RefreshToken = authResponse.RefreshToken;
                await _userManager.UpdateAsync(user);
                return Ok(authResponse);
            }
            else
            {
                return Unauthorized("Invalid credentials");
            }

        }
        /*---------------*/
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestErrorMessages();
            }

            //fetch email from expired token string
            var principal = GetPrincipalFromExpiredToken(request.Token);
            var userEmail = principal.FindFirstValue("Email"); //fetch the email claim's value

            //check if any user with this refresh token exists
            //var user = await GetUserByRefreshToken(request.RefreshToken);
            //if (user == null)
            var user = !string.IsNullOrEmpty(userEmail) ? await _userManager.FindByEmailAsync(userEmail) : null;
            if (user == null || user.RefreshToken != request.RefreshToken)
            {
                return BadRequest("Invalid refresh token");
            }

            //provide new access and refresh tokens
            var response = await GetTokens(user);
            user.RefreshToken = response.RefreshToken;
            await _userManager.UpdateAsync(user);
            return Ok(response);
        }
        /*--------------------*/
        [HttpGet("tokenValidate")]
        [Authorize]
        public async Task<IActionResult> TokenValidate()
        {
            //This endpoint is created so any user can validate their token
            return Ok("Token is valid");
        }
        /*------------*/
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestErrorMessages();
            }

            var isEmailAlreadyRegistered = await _userRepo.GetUserAsync(PropertyField.email, registerRequest.Email) != null;
            var isUserNameAlreadyRegistered = await _userRepo.GetUserAsync(PropertyField.username, registerRequest.Email) != null;

            if (isEmailAlreadyRegistered)
            {
                return Conflict($"Email Id {registerRequest.Email} is already registered.");
            }
            if (isUserNameAlreadyRegistered)
            {
                return Conflict($"Username {registerRequest.Username} is already registered");
            }

            //var newUser = new User
            //{
            //    Email = registerRequest.Email,
            //    UserName = registerRequest.Username,
            //    DisplayName = registerRequest.DisplayName,
            //};

            var result = await AddUser(registerRequest, false);
            if (result.Succeeded)
            {
                return Ok("User created successfully");
            }
            else
            {
                return StatusCode(500, result.Errors.Select(e => new { Msg = e.Code, Desc = e.Description }).ToList());
            }

            

        }
        /*------------------------------*/
        [Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(RevokeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestErrorMessages();
            }

            //fetch email from claims of currently logged in user
            var userEmail = this.HttpContext.User.FindFirstValue("Email");

            //check if any user with this refresh token exists
                    //var user = await GetUserByRefreshToken(request.RefreshToken);
                    //if (user == null)
            var user = !string.IsNullOrEmpty(userEmail) ? await _userManager.FindByEmailAsync(userEmail) : null;
            if (user == null || user.RefreshToken != request.RefreshToken)
            {
                return BadRequest("Invalid refresh token");
            }

            //remove refresh token 
            user.RefreshToken = String.Empty; //HACK original guide had this set to null. I'm setting it to String.Empty
            await _userManager.UpdateAsync(user);
            return Ok("Refresh token is revoked");
        }





        private async Task<ApplicationUser> AddUser(RegisterModel registerModel)
        {
            var newUser = await _userRepo.CreateUserDexHolderAsync(registerModel);
            return newUser;
        }
        private async Task<IdentityResult> AddUser(RegisterModel registerModel, bool tf)
        {
            var result = await _userRepo.CreateUserDexHolderAsync(registerModel, tf);
            return (IdentityResult)result;
        }
        private async Task<ApplicationUser?> GetUserByEmail(string email)
        {
            return await _userRepo.GetUserAsync(PropertyField.email, email);
        }
        /*-------------------------------*/

        private async Task<ApplicationUser?> GetUser(string email, string password)
        {
            return await _userRepo.GetUserAsync(PropertyField.email, email, password);
        }


        private IActionResult BadRequestErrorMessages()
        {
            var errMsgs = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(errMsgs);
        }




        private async Task<AuthenticatedResponse> GetTokens(ApplicationUser user)
        {
            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["JwtSettings:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:MinutesToExpiration"])),
                signingCredentials: signIn);
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshTokenStr = GetRefreshToken();
            user.RefreshToken = refreshTokenStr;
            var authResponse = new AuthResponse { Token = tokenStr, RefreshToken = refreshTokenStr };
            return await Task.FromResult(authResponse);
        }
        /*------------------------*/
        private string GetRefreshToken()
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            // ensure token is unique by checking against db
            var tokenIsUnique = !_userManager.Users.Any(u => u.RefreshToken == token);
            //var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == request.RefreshToken);



            if (!tokenIsUnique)
                return GetRefreshToken();  //recursive call

            return token;
        }
        private async Task<ApplicationUser> GetUserByRefreshToken(string refreshToken)
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
