using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetDexTest_01.Services;
using System.Collections.Generic;
using NetDexTest_01.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NetDexTest_01.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using System.Text.Json;
using NetDexTest_01.Models.ViewModels;

// https://memorycrypt.hashnode.dev/create-a-web-api-with-jwt-authentication-and-aspnet-core-identity

namespace NetDexTest_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<UserController> _logger;
        private readonly JwtTokenCreator _jwtCreator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(ILogger<UserController> logger, JwtTokenCreator jwtCreator, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                                IUserRepository userRepo, IUserService userService)
        {
            _userRepo = userRepo;
            _userService = userService;
            _logger = logger;
            _jwtCreator = jwtCreator;
            _userManager = userManager;
            _signInManager = signInManager;

        }


        // TODO - Modify
        // - create register model

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterModel model)
        {
            var result = await _userService.RegisterAsync(model);
            return Ok(result);
        }



        [HttpPost("token")]
        public async Task<IActionResult> GetTokenAsync(TokenRequestModel model)
        {
            var result = await _userService.GetTokenAsync(model);
            return Ok(result);
        }


        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync(AddRoleModel model)
        {
            var result = await _userService.AddRoleAsync(model);
            return Ok(result);
        }



        //[HttpPost("login")]
        //public IActionResult Login([FromBody] LoginModel user)
        //{
        //    if (user is null)
        //    {
        //        return BadRequest("Invalid client request");
        //    }
        //    if (user.Username == "johndoe" && user.Password == "def@123")
        //    {


        //        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        //        return Ok(new AuthenticatedResponse { Token = tokenString });
        //    }
        //    return Unauthorized();
        //}



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("ping")]
        public string Ping() => "pong";


/*----------------------------*/

        // GET: api/user/one/{id}
        [HttpGet("one/{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var user = await _userRepo.ReadByIdAsync(id);

            if (user == null)
            {
                user = await _userRepo.ReadByUsernameAsync(id);
                if (user == null)
                {
                    user = await _userRepo.GetByEmailAsync(id);
                }
            }
            if (user == null) return NotFound();

            return Ok(user);
        }


        [HttpGet("dex/{id}")]
        public async Task<IActionResult> GetOneDex(string id)
        {
            // id should work as a username, userId, and an Email
            var user = await _userRepo.GetDexHolderMiddleVMAsync(id);


            if (user == null) return NotFound();

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles, //IgnoreCycles, //Preserve
                WriteIndented = true
            };

            string modelJson = JsonSerializer.Serialize(user, options);

            //var model = all;

            await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {modelJson}\n \n\n\n\n");

            var model = modelJson;
            return Ok(model);
        }



        // GET: api/user/admin/all
        [HttpGet("admin/all")]
        public async Task<IActionResult> GetAll()
        {
            //var user = await _userRepo.ReadByIdAsync(id);

            ICollection<AdminUserVM> users = await _userRepo.ReadAllApplicationUsersVMAsync();

            //if (users == null)
            //{
            //    return NotFound();
            //}

            //List<AdminUserVMOut> usersOut = new();

            //foreach (AdminUserVM user in users) {
            //    usersOut.Add(new AdminUserVMOut(user));
            //}

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles, //IgnoreCycles, //Preserve
                WriteIndented = true
            };

            string modelJson = JsonSerializer.Serialize(users, options);

            //var model = all;

            await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {modelJson}\n \n\n\n\n");

            var model = modelJson;
            return Ok(model);

            //return Ok(users);
        }


        /*----------------------------*/

        [HttpPost("registerform")]
        public async Task<ActionResult> RegisterFormAsync([FromForm] RegisterModel user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdUser = await _userService.CreateRegisterAsync(user);
            if (createdUser != null)
            { 
                return CreatedAtAction(nameof(GetOne), new { id = createdUser.Id }, createdUser);
            }
            return BadRequest($"User {user.Username} with email {user.Email} already exists!"); 

        }



        [HttpPost("login")]
        public async Task<IActionResult> LoginApi([FromBody] LoginModel model)
        {
            var test = await _userRepo.GetByUsernameAsync(model.Username);
            if (model.Username == null || test == null)
            {
                var tempuser = await _userManager.FindByEmailAsync(model.Email); // changed from FindByEmailAsync
                if (tempuser != null) model.Username = tempuser.UserName; 
            }


            if (ModelState.IsValid)
            {
                
                var signIn = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
                if (signIn.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username); // changed from FindByEmailAsync
                    if (user == null)
                    {
                       user = await _userManager.FindByEmailAsync(model.Email); // changed from FindByEmailAsync
                    }
                    
                    var token = _jwtCreator.Generate(user.Email, user.Id);

                    user.RefreshToken = Guid.NewGuid().ToString();

                    await _userManager.UpdateAsync(user);

                    Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                    Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                    Response.Cookies.Append("X-Email", user.Email, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                    Response.Cookies.Append("X-Refresh-Token", user.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

                    return Ok(user);
                }
                else
                {
                    return BadRequest(new { signIn.IsLockedOut, signIn.IsNotAllowed, signIn.RequiresTwoFactor });
                }
            }
            else
                return BadRequest(ModelState);
        }

        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!(Request.Cookies.TryGetValue("X-Username", out var userName) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
                return BadRequest();

            var user = _userManager.Users.FirstOrDefault(i => i.UserName == userName && i.RefreshToken == refreshToken);

            if (user == null)
                return BadRequest();

            var token = _jwtCreator.Generate(user.Email, user.Id);

            user.RefreshToken = Guid.NewGuid().ToString();

            await _userManager.UpdateAsync(user);

            Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Refresh-Token", user.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok();
        }











    }




}

