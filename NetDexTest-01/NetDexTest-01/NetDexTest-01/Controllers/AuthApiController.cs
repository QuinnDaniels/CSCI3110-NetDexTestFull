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
    public partial class AuthController : ControllerBase
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








    }
}
