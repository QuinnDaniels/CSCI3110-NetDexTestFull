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
using Microsoft.EntityFrameworkCore;

// https://memorycrypt.hashnode.dev/create-a-web-api-with-jwt-authentication-and-aspnet-core-identity

namespace NetDexTest_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepo;
        private readonly IPersonRepository _personRepo;
        private readonly ILogger<UserController> _logger;
        private readonly JwtTokenCreator _jwtCreator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserController(ILogger<UserController> logger, JwtTokenCreator jwtCreator,
                        ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                                IUserRepository userRepo, IPersonRepository personRepo, IUserService userService)
        {
            _userRepo = userRepo;
            _personRepo = personRepo;
            _userService = userService;
            _logger = logger;
            _jwtCreator = jwtCreator;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

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


            if (user == null) return NotFound();

            //JsonSerializerOptions options = new()
            //{
            //    ReferenceHandler = ReferenceHandler.IgnoreCycles, //IgnoreCycles, //Preserve
            //    WriteIndented = true
            //};
            //var peopleList = await _personRepo.ReadAllPeopleAsync(user);

            //DexHolderMiddleVM outer = new()
            //{
            //    ApplicationEmail = user.Email,
            //    ApplicationUserName = user.UserName,
            //    People = peopleList
                
            //};
            
            //string modelJson = JsonSerializer.Serialize(outer, options);

            //await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {modelJson}\n \n\n\n\n");

            //var model = modelJson;
            //return Ok(model);


            return Ok(new//user
            {
                ApplicationEmail = user.Email,
                Username = user.UserName//,
                //People = user.DexHolder.People
            });
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




        [HttpGet("find/{id}")]
        public async Task<IActionResult> GetSingleDex(string id)
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
            return Ok(user);
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






        [HttpDelete("delete/{input}")]
        public async Task<IActionResult> DeleteOneUser(string input)
        {
            // id should work as a username, userId, and an Email
            var userVM = await _userRepo.GetDexHolderMiddleVMAsync(input);


            if (userVM == null) return NotFound();

            var userToDelete = _context.Users.FirstOrDefault(a => a.Id.Equals(userVM.ApplicationUserId));

            if (userToDelete == null)
            {
                return NotFound();
            }

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return NoContent();

        }



        private bool UserExists(string id)
        {
            var userToFind = _context.Users.FirstOrDefault(a => a.Id.Equals(id));

            return _context.Users.Any(e => e.Id == id);
        }
        private bool DexExists(int id)
        {
            var userToFind = _context.DexHolder.FirstOrDefault(a => a.Id.Equals(id));

            return _context.DexHolder.Any(e => e.Id == id);
        }



        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateUser([FromForm] DexHolderUserEditVM userEdit, string id) // TODO: add DexHolder id
        {
            await Console.Out.WriteLineAsync("\n\n\n ----------------------- \n\n UpdateUser Endpoint Reached! \n\n -------------------\n\n ");

            userEdit.ApplicationUserId = userEdit.ApplicationUserId!.Trim();
            

            await Console.Out.WriteLineAsync($"\n\t {userEdit?.ApplicationUserId ?? "ApplicationUserId is null!"}  ");
            await Console.Out.WriteLineAsync($"\n\t {userEdit?.ApplicationUserName ?? "ApplicationUserName is null!"}  ");
            await Console.Out.WriteLineAsync($"\n\t {userEdit?.ApplicationEmail ?? "ApplicationEmail is null!"}    ");
            await Console.Out.WriteLineAsync($"\n\t {userEdit?.FirstName ?? "FirstName is null!"} ");
            await Console.Out.WriteLineAsync($"\n\t {userEdit?.MiddleName ?? "MiddleName is null!"} ");
            await Console.Out.WriteLineAsync($"\n\t {userEdit?.LastName ?? "LastName is null!"} ");
            await Console.Out.WriteLineAsync($"\n\t {userEdit?.Gender ?? "Gender is null!"} ");
            await Console.Out.WriteLineAsync($"\n\t {userEdit?.Pronouns ?? "Pronouns is null!"} ");
            await Console.Out.WriteLineAsync($"\n\t {userEdit?.DateOfBirth  ?? DateTime.UnixEpoch} ");

            var userVM = await _userRepo.GetDexHolderMiddleVMAsync(id);

            var input = id;
            //// return a bad request if userVM
            if (userVM == null )
            {
                return BadRequest($"User not found via the input, {input}");
            }
            if (userEdit.DexId != userVM.DexId && userEdit.ApplicationUserId != userVM.ApplicationUserId)
            {
                return BadRequest("There is a mismatch in the DexId and in the UserId!! ");
            }
            if (userEdit.ApplicationUserId != userVM.ApplicationUserId)
            {
                return BadRequest("There is a mismatch in the UserId! ");
            }
            if (userEdit.DexId != userVM.DexId)
            {
                return BadRequest("There is a mismatch in the DexId! ");
            }


            // HACK
            userEdit.ApplicationUserName = userEdit?.ApplicationUserName ?? userVM.ApplicationUserName;
            userEdit.ApplicationEmail = userEdit?.ApplicationEmail ?? userVM.ApplicationEmail;
            userEdit.FirstName = userEdit?.FirstName ?? userVM.FirstName;
            userEdit.MiddleName = userEdit?.MiddleName ?? userVM.MiddleName;
            userEdit.LastName = userEdit?.LastName ?? userVM.LastName;
            userEdit.Gender = userEdit?.Gender ?? userVM.Gender;
            userEdit.Pronouns = userEdit?.Pronouns ?? userVM.Pronouns;
            userEdit.DateOfBirth =userEdit?.DateOfBirth ?? userVM.DateOfBirth;



            var userToUpdate = _context.Users.Include(u => u.DexHolder).FirstOrDefault(a => a.Id.Equals(userEdit.ApplicationUserId) || a.Id == userEdit.ApplicationUserId);
            var dexToUpdate = _context.DexHolder.Include(dh => dh.ApplicationUser).FirstOrDefault(dh => dh.Id.Equals(userEdit.DexId) || dh.ApplicationUser.Id == userEdit.ApplicationUserId);

            if (userToUpdate == null || dexToUpdate == null) // added to conform to tutorial. is likely redundant, considering scaffold result
            {
                return NotFound($"Could not find both a user and dexholder"); // for id, {person.Id}\n{person.Nickname}\n{person.DateOfBirth}\n{person.Gender}\n{person.Pronouns}\n{person.Rating}\n{person.Favorite}\n\n");
            }



            //HACK - add this back later
            /*
            //TODO replace this with an email check?
            if (id != personToUpdate.Id) //(id != person.Id.ToString())
            {
                return BadRequest($"id, {id}, did not match personToUpdate id, {personToUpdate.Id}. Person.Id: {person.Id}");
            }


            //if (id != person.Id) //(id != person.Id.ToString())
            //{
            //    return BadRequest($"id, {id}, did not match person id, {person.Id}. PersonToUpdate id: {personToUpdate.Id}");
            //}
             */


            await Console.Out.WriteLineAsync("\n\n\n ----------------------- \n\n Conditional Checks Passed! \n\n -------------------\n\n ");

            _context.Entry(userToUpdate).State = EntityState.Modified;
            _context.Entry(dexToUpdate).State = EntityState.Modified;

            try
            {
                //personToUpdate.DexHolder = person.DexHolder ;
                
                //ApplicationUserName
                //ApplicationEmail
                //FirstName
                //MiddleName
                //LastName
                //Gender
                //Pronouns
                //DateOfBirth

                if (userEdit.ApplicationUserName != userToUpdate.UserName)
                {
                    var tUser = await _userRepo.GetByIdAsync(userEdit.ApplicationUserId);
                    await Console.Out.WriteLineAsync($"\n\t " +
                        $"{userToUpdate.UserName} => " +
                        $"{userEdit?.ApplicationUserName ?? userToUpdate.UserName}\n  ");
                    await _userManager.SetUserNameAsync(tUser, userEdit.ApplicationUserName);
                    
                    userToUpdate.UserName = userEdit?.ApplicationUserName ?? userToUpdate.UserName;
                    dexToUpdate.ApplicationUserName = userEdit?.ApplicationUserName ?? dexToUpdate.ApplicationUserName;

                }
                if (userEdit.ApplicationEmail != userToUpdate.Email)
                {
                    var tUser = await _userRepo.GetByIdAsync(userEdit.ApplicationUserId);
                    await Console.Out.WriteLineAsync($"\n\t " +
                        $"{userToUpdate.Email} => " +
                        $"{userEdit?.ApplicationEmail ?? userToUpdate.Email}\n  ");
                    await _userManager.SetEmailAsync(tUser, userEdit.ApplicationEmail);
                    userToUpdate.Email = userEdit?.ApplicationEmail ?? userToUpdate.Email;
                }

                if (userEdit.FirstName != dexToUpdate.FirstName)
                {
                    await Console.Out.WriteLineAsync($"\n\t " +
                        $"{dexToUpdate.FirstName} => " +
                        $"{userEdit?.FirstName ?? dexToUpdate.FirstName}\n  ");
                    dexToUpdate.FirstName= userEdit?.FirstName ?? dexToUpdate.FirstName;
                }
                if (userEdit.MiddleName != dexToUpdate.MiddleName)
                {
                    await Console.Out.WriteLineAsync($"\n\t " +
                        $"{dexToUpdate.MiddleName} => " +
                        $"{userEdit?.MiddleName ?? dexToUpdate.MiddleName}\n  ");
                    dexToUpdate.MiddleName= userEdit?.MiddleName ?? dexToUpdate.MiddleName;
                }
                if (userEdit.LastName != dexToUpdate.LastName)
                {
                    await Console.Out.WriteLineAsync($"\n\t " +
                        $"{dexToUpdate.LastName} => " +
                        $"{userEdit?.LastName ?? dexToUpdate.LastName}\n  ");
                    dexToUpdate.LastName = userEdit?.LastName ?? dexToUpdate.LastName;
                }
                if (userEdit.Gender != dexToUpdate.Gender)
                {
                    await Console.Out.WriteLineAsync($"\n\t " +
                        $"{dexToUpdate.Gender} => " +
                        $"{userEdit?.Gender ?? dexToUpdate.Gender}\n  ");
                    dexToUpdate.Gender = userEdit?.Gender ?? dexToUpdate.Gender;
                }
                if (userEdit.Pronouns != dexToUpdate.Pronouns)
                {
                    await Console.Out.WriteLineAsync($"\n\t " +
                        $"{dexToUpdate.Pronouns} => " +
                        $"{userEdit?.Pronouns ?? dexToUpdate.Pronouns}\n  ");
                    dexToUpdate.Pronouns = userEdit?.Pronouns ?? dexToUpdate.Pronouns;
                }
                if (userEdit.DateOfBirth != dexToUpdate.DateOfBirth)
                {
                    await Console.Out.WriteLineAsync($"\n\t " +
                        $"{dexToUpdate.DateOfBirth} => " +
                        $"{userEdit?.DateOfBirth ?? dexToUpdate.DateOfBirth}\n  ");
                    dexToUpdate.DateOfBirth = userEdit?.DateOfBirth ?? dexToUpdate.DateOfBirth;
                }


                _context.Users.Update(userToUpdate);
                _context.DexHolder.Update(dexToUpdate);

                await _context.SaveChangesAsync();
                await Console.Out.WriteLineAsync("\n\n\n ----------------------- \n\n Changes saved to context! \n\n -------------------\n\n ");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                if (!UserExists(userEdit.ApplicationUserId))
                {
                    return NotFound();
                }
                if (!DexExists(userEdit.DexId))
                {
                    return NotFound();
                }
                else
                {
                    //throw;
                    /*
                    //await Console.Out.WriteLineAsync($"\n\n\t {personToUpdate.Nickname} -> {person?.Nickname ?? personToUpdate.Nickname}\n  ");
                    //personToUpdate.Nickname = person?.Nickname ?? personToUpdate.Nickname;
                    ////personToUpdate.DexHolder = person.DexHolder ;
                    //await Console.Out.WriteLineAsync($"\n\n\t {personToUpdate.DateOfBirth} => {person?.DateOfBirth ?? personToUpdate.DateOfBirth}\n  ");
                    //personToUpdate.DateOfBirth = person?.DateOfBirth ?? personToUpdate.DateOfBirth;
                    //await Console.Out.WriteLineAsync($"\n\n\t {personToUpdate.Gender} => {person?.Gender ?? personToUpdate.Gender}\n  ");
                    //personToUpdate.Gender = person?.Gender ?? personToUpdate.Gender;
                    //await Console.Out.WriteLineAsync($"\n\n\t {personToUpdate.Pronouns} => {person?.Pronouns ?? personToUpdate.Pronouns}\n  ");
                    //personToUpdate.Pronouns = person?.Pronouns ?? personToUpdate.Pronouns;
                    //await Console.Out.WriteLineAsync($"\n\n\t {personToUpdate.Rating} => {person?.Rating ?? personToUpdate.Rating}\n  ");
                    //personToUpdate.Rating = person?.Rating ?? personToUpdate.Rating;
                    //await Console.Out.WriteLineAsync($"\n\n\t {personToUpdate.Favorite} => {person?.Favorite ?? personToUpdate.Favorite}\n  ");
                    //personToUpdate.Favorite = person?.Favorite ?? personToUpdate.Favorite;

                    //_context.Person.Update(personToUpdate);
                    //_context.SaveChanges();
                     */


                    if (userEdit.ApplicationUserName != userToUpdate.UserName)
                    {
                        var tUser = await _userRepo.GetByIdAsync(userEdit.ApplicationUserId);
                        await Console.Out.WriteLineAsync($"\n\t " +
                            $"{userToUpdate.UserName} => " +
                            $"{userEdit?.ApplicationUserName ?? userToUpdate.UserName}\n  ");
                        await _userManager.SetUserNameAsync(tUser, userEdit.ApplicationUserName);

                        userToUpdate.UserName = userEdit?.ApplicationUserName ?? userToUpdate.UserName;
                        dexToUpdate.ApplicationUserName = userEdit?.ApplicationUserName ?? dexToUpdate.ApplicationUserName;

                    }
                    if (userEdit.ApplicationEmail != userToUpdate.Email)
                    {
                        var tUser = await _userRepo.GetByIdAsync(userEdit.ApplicationUserId);
                        await Console.Out.WriteLineAsync($"\n\t " +
                            $"{userToUpdate.Email} => " +
                            $"{userEdit?.ApplicationEmail ?? userToUpdate.Email}\n  ");
                        await _userManager.SetEmailAsync(tUser, userEdit.ApplicationEmail);
                        userToUpdate.Email = userEdit?.ApplicationEmail ?? userToUpdate.Email;
                    }

                    if (userEdit.FirstName != dexToUpdate.FirstName)
                    {
                        await Console.Out.WriteLineAsync($"\n\t " +
                            $"{dexToUpdate.FirstName} => " +
                            $"{userEdit?.FirstName ?? dexToUpdate.FirstName}\n  ");
                        dexToUpdate.FirstName = userEdit?.FirstName ?? dexToUpdate.FirstName;
                    }
                    if (userEdit.MiddleName != dexToUpdate.MiddleName)
                    {
                        await Console.Out.WriteLineAsync($"\n\t " +
                            $"{dexToUpdate.MiddleName} => " +
                            $"{userEdit?.MiddleName ?? dexToUpdate.MiddleName}\n  ");
                        dexToUpdate.MiddleName = userEdit?.MiddleName ?? dexToUpdate.MiddleName;
                    }
                    if (userEdit.LastName != dexToUpdate.LastName)
                    {
                        await Console.Out.WriteLineAsync($"\n\t " +
                            $"{dexToUpdate.LastName} => " +
                            $"{userEdit?.LastName ?? dexToUpdate.LastName}\n  ");
                        dexToUpdate.LastName = userEdit?.LastName ?? dexToUpdate.LastName;
                    }
                    if (userEdit.Gender != dexToUpdate.Gender)
                    {
                        await Console.Out.WriteLineAsync($"\n\t " +
                            $"{dexToUpdate.Gender} => " +
                            $"{userEdit?.Gender ?? dexToUpdate.Gender}\n  ");
                        dexToUpdate.Gender = userEdit?.Gender ?? dexToUpdate.Gender;
                    }
                    if (userEdit.Pronouns != dexToUpdate.Pronouns)
                    {
                        await Console.Out.WriteLineAsync($"\n\t " +
                            $"{dexToUpdate.Pronouns} => " +
                            $"{userEdit?.Pronouns ?? dexToUpdate.Pronouns}\n  ");
                        dexToUpdate.Pronouns = userEdit?.Pronouns ?? dexToUpdate.Pronouns;
                    }
                    if (userEdit.DateOfBirth != dexToUpdate.DateOfBirth)
                    {
                        await Console.Out.WriteLineAsync($"\n\t " +
                            $"{dexToUpdate.DateOfBirth} => " +
                            $"{userEdit?.DateOfBirth ?? dexToUpdate.DateOfBirth}\n  ");
                        dexToUpdate.DateOfBirth = userEdit?.DateOfBirth ?? dexToUpdate.DateOfBirth;
                    }


                    _context.Users.Update(userToUpdate);
                    _context.DexHolder.Update(dexToUpdate);

                    await _context.SaveChangesAsync();

                }
            }

            return NoContent();
        }








    }




}

