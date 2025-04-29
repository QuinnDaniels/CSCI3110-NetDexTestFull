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
using NuGet.Common;
using static NetDexTest_01.Constants.Authorization;

namespace NetDexTest_01.Controllers
{
    [ApiController]
    [Route("auth")]
    public partial class AuthController : ControllerBase
    {
        //public static List<User> tempUserDb = new List<User>{
        //    new User{UserId="abc123",UserName="John", DisplayName="BilboBaggins", Email="john@abc.com", Password="john@123" },
        //    new User{UserId="def456",UserName="Jane", DisplayName="Galadriel", Email="jane@xyz.com", Password="jane1995" }
        //};

        public IConfiguration _configuration;
        private UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepo;
        private readonly IUserService _userService;
        private readonly IDebugRepository _debug;



        /*---------------*/
        public AuthController(IDebugRepository debug, IUserRepository userRepo, IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _configuration = config;
            _userManager = userManager;
            _userRepo = userRepo;
            _debug = debug;
        }
        /*---------------*/
        //[HttpPost("loginold")]
        //public async Task<IActionResult> Login(LoginRequest request)
        //{
        //    await Console.Out.WriteAsync($"\n\n\n\n----------AuthApiController - Login------------\n\n\n");

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequestErrorMessages();
        //    }
        //    await Console.Out.WriteAsync($"\nrequest:\n\t\t{request.Email}\n\n");
        //    await Console.Out.WriteAsync($"\nrequest:\n\t\t{request.Password}\n\n");

        //    //var user = await _userRepo.GetUserAsync(PropertyField.email, request.Email, request.Password)
        //    //var user = await GetUser(request.Email, request.Password);
        //    var user = await _userManager.FindByEmailAsync(request.Email);
        //    await Console.Out.WriteAsync($"\nuser:\n\t\t{user.UserName}\n");
        //    await Console.Out.WriteAsync($"\nuser:\n\t\t{user.Email}\n");
        //    //await Console.Out.WriteAsync($"\nuser:\n\t\t{user.DexHolder.Id}\n"); // "Object reference not set to an instance of an object."
        //    var isAuthorized = user != null && await _userManager.CheckPasswordAsync(user, request.Password);

        //    //if (user != null)
        //    if (isAuthorized)
        //    {
        //        var adapter = new TokenRequestModel(request.Email, request.Password);

        //        var authResponse = await _userRepo.GetTokens(user); //_userService.GetTokenAsync(adapter); //_userRepo.GetTokens(user);
        //        await Console.Out.WriteAsync($"\nauthResponse Token:\n\t\t{authResponse.Token}\n");
        //        user.RefreshToken = authResponse.RefreshToken;
        //        await _userManager.UpdateAsync(user);
        //        await Console.Out.WriteAsync($"\nauthResponse:\n\t{authResponse.ToString()}\n");
        //        await Console.Out.WriteAsync($"\n\n\n\n----END---AuthApiController - Login------------\n\n\n");
        //        return Ok(authResponse);
        //    }
        //    else
        //    {
        //        return Unauthorized("Invalid credentials");
        //    }

        //}
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

            // TODO add role??
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



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid credentials.");
            }

            // Generate access & refresh tokens
            var tokenResponse =  await _userRepo.GetTokens(user); //_tokenService.GenerateAccessToken(user);
            
            var rolesList = await _userManager.GetRolesAsync(user);//.ConfigureAwait(false);
            tokenResponse.Roles = rolesList.ToList();

            //var refreshToken = _userRepo.GetRefreshToken(); // _tokenService.GenerateRefreshToken(user);

            // Optional: set refresh token in cookie
            Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, //refreshToken,
                new CookieOptions
            {
                HttpOnly = true,
                //Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
            if(user != null)
            {
                Response.Cookies.Append("Username", user.UserName,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
            }
            if (tokenResponse.Token != null)
            {
                Response.Cookies.Append("Access-Token", tokenResponse.Token,
                new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict 
                });
            }
            if (tokenResponse.UserOut != null)
            {
            Response.Cookies.Append("Email", tokenResponse.UserOut,
                new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict
                });
            }


            // Return tokens in body for MVC app to consume
            return Ok(new
            {
                Email = user.Email, //tokenResponse.UserOut
                Roles = tokenResponse.Roles,
                AccessToken = tokenResponse.Token,
                RefreshToken = tokenResponse.RefreshToken //refreshToken //tokenResponse.RefreshToken
            });
        }


        [HttpPost("login2")]
        public async Task<IActionResult> Login2([FromBody] LoginModel model)
        {
            ApplicationUser? user = null;
            if(model.Email != null)
            {
                user = await _userManager.FindByEmailAsync(model.Email);
            }
            if (model.Username != null && user == null)
            {
                user = await _userManager.FindByNameAsync(model.Username);
            }
            if (user == null && model.Input != null )
            {
                user = await _userManager.FindByNameAsync(model.Input);
                if (user == null) { user = await _userManager.FindByEmailAsync(model.Input); }
            }
            
            if (user == null || model.Password == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid credentials.");
            }

            // Generate access & refresh tokens
            var tokenResponse = await _userRepo.GetTokens(user); //_tokenService.GenerateAccessToken(user);

            var rolesList = await _userManager.GetRolesAsync(user);//.ConfigureAwait(false);
            //tokenResponse.Roles = rolesList.ToList();
            tokenResponse.Roles = rolesList.ToList();



            string rolesString = rolesList.ToList().Aggregate("", (current, s) => current + (s + "+"));


            //var refreshToken = _userRepo.GetRefreshToken(); // _tokenService.GenerateRefreshToken(user);

            // Optional: set refresh token in cookie
            Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, //refreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    //Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
            if (user != null)
            {
                Response.Cookies.Append("Username", user.UserName,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
            }
            if (tokenResponse.Token != null)
            {
                Response.Cookies.Append("Access-Token", tokenResponse.Token,
                new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict
                });
            }
            if (tokenResponse.UserOut != null)
            {
                Response.Cookies.Append("Email", tokenResponse.UserOut,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
            }
            if (tokenResponse.UserOut != null)
            {
                Response.Cookies.Append("Roles", rolesString, //tokenResponse.Roles,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
            }


            // Return tokens in body for MVC app to consume
            return Ok(new
            {
                Email = user.Email, //tokenResponse.UserOut
                Username = user.UserName,
                Roles = rolesString, //tokenResponse.Roles,
                AccessToken = tokenResponse.Token,
                RefreshToken = tokenResponse.RefreshToken //refreshToken //tokenResponse.RefreshToken
            });;
        }


        [HttpPost("loginDouble")]
        public async Task<IActionResult> LoginDouble([FromBody] LoginModel model)
        {
            ApplicationUser? user = null;
            ApplicationUser? userCompare = null;
            bool compareFlag = !true;
            string badMessage = "";
            if (model.Email != null && model.Username != null)
            {
                user = await _userManager.FindByEmailAsync(model.Email);
                userCompare = await _userManager.FindByNameAsync(model.Username);

                if(user!=null && userCompare != null)
                {
                    if (user.Email != userCompare.Email || user.UserName != userCompare.UserName)
                    {
                        badMessage = "\n\tEmail and Username do not match the same user! ";
                        compareFlag = !false;
                    }
                }

            }
            if (model.Username == null) { badMessage = badMessage + "\n\tUsername cannot be null! "; }
            if (model.Email == null) { badMessage = badMessage + "\n\tEmail cannot be null! "; }
            if(user == null) { badMessage = badMessage + "\n\tEmail did not match a user! ";  }
            if (userCompare == null) { badMessage = badMessage + "\n\tUsername did not match a user! ";  }

            if (compareFlag == !false) { user = null; }
            if (model.Input != null) { badMessage = badMessage + "\nInput was unnecessary, but it didn't cause an error... ";  }
            

            if(user == null) { return Unauthorized($"Invalid credentials.\n{badMessage}"); }

            if (model.Password == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid credentials.");
            }

            // Generate access & refresh tokens
            var tokenResponse = await _userRepo.GetTokens(user); //_tokenService.GenerateAccessToken(user);

            var rolesList = await _userManager.GetRolesAsync(user);//.ConfigureAwait(false);
            //tokenResponse.Roles = rolesList.ToList();
            tokenResponse.Roles = rolesList.ToList();



            string rolesString = rolesList.ToList().Aggregate("", (current, s) => current + (s + "+"));


            //var refreshToken = _userRepo.GetRefreshToken(); // _tokenService.GenerateRefreshToken(user);

            // Optional: set refresh token in cookie
            Response.Cookies.Append("RefreshToken", tokenResponse.RefreshToken, //refreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    //Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
            if (user != null)
            {
                Response.Cookies.Append("Username", user.UserName,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
            }
            if (tokenResponse.Token != null)
            {
                Response.Cookies.Append("Access-Token", tokenResponse.Token,
                new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict
                });
            }
            if (tokenResponse.UserOut != null)
            {
                Response.Cookies.Append("Email", tokenResponse.UserOut,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
            }
            if (tokenResponse.UserOut != null)
            {
                Response.Cookies.Append("Roles", rolesString, //tokenResponse.Roles,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
            }


            // Return tokens in body for MVC app to consume
            return Ok(new
            {
                Email = user.Email, //tokenResponse.UserOut
                Username = user.UserName,
                Roles = rolesString, //tokenResponse.Roles,
                AccessToken = tokenResponse.Token,
                RefreshToken = tokenResponse.RefreshToken //refreshToken //tokenResponse.RefreshToken
            }); ;
        }



    }
}
