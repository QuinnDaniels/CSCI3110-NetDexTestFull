//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDexTest_01_MVC.Controllers;
using NetDexTest_01_MVC.Models;
using NetDexTest_01_MVC.Models.Authentication;
using NetDexTest_01_MVC.Services;
using System.Diagnostics;
using System.Net;


namespace NetDexTest_01_MVC.Controllers
{
    /// <summary>
    /// <seealso href="https://memorycrypt.hashnode.dev/net-mvc-app-calling-web-api-for-authentication#heading-2-add-web-api-urls-in-appsettingsjson">
    /// </summary>
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private IAuthService _authService;
        private readonly IUserSessionService _userSessionService;


        public AuthController(ILogger<AuthController> logger,
            IAuthService authService,
            IUserSessionService userSessionService)
        {
            _logger = logger;
            _authService = authService;
            _userSessionService = userSessionService;

        }

        public async Task<IActionResult> Index()
        {
            await Console.Out.WriteLineAsync($"\n\n--------GET /auth/index Reached!---------\n\n");

            return View();
        }




        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestModel registerRequest)
        {
            if (!ModelState.IsValid)
            {
                //return error messages
                return View(registerRequest);
            }
            else
            {
                //register the user info
                var response = await _authService.RegisterAsync(registerRequest);

                if (response.Status != HttpStatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(registerRequest);
                }

                //return success message on browser
                //return Ok("Registration Successful");

                //silent login
                var loginRequest = new LoginRequest { Email = registerRequest.Email, Password = registerRequest.Password };
                return await Login(loginRequest);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await Console.Out.WriteLineAsync($"\n\n--------GET /auth/login Reached!---------\n\n");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm]LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                //return error messages
                return View(loginRequest);
            }
            else
            {
                //log the user in
                //HACK                          // HACK
                var response = await _authService.LoginAsyncAlternative(loginRequest); //FIXME - auth token not populated???
                //var response = await _authService.LoginAsyncAlternative(loginRequest); //FIXME - auth token not populated???

                await Console.Out.WriteLineAsync($"\n\n--------POST /auth/login --> login > var response set!---------");
                await Console.Out.WriteLineAsync($"\n\n\nresponse: {response.Status}\n");
                await Console.Out.WriteLineAsync($"\nresponse: {response.Message} \n");
                await Console.Out.WriteLineAsync($"\nresponse: {response.Email}\n");
                await Console.Out.WriteLineAsync($"\nresponse: {response.Username}\n");
                await Console.Out.WriteLineAsync($"\nresponse: {response.AccessToken}\n");
                await Console.Out.WriteLineAsync($"\nresponse: {response.RefreshToken}\n\n\n");
                await Console.Out.WriteLineAsync($"\nresponse: {response.Roles}\n\n\n");
                await Console.Out.WriteLineAsync($"\n------------------------------------\n\n\n");


                if (response.AccessToken != null && response.RefreshToken != null)//response.Status == HttpStatusCode.OK)
                {
                    //Instruct the browser to store the auth tokens in a cookie
                    //TODO
                    try
                    {

                        //generate claims for email, access token, and refresh token
                        await _authService.LoginWithCookieAsync(response.Email, response.AccessToken, response.RefreshToken);

                        //await _authService.LoginWithCookieAsync(response.Email, response.AccessToken, response.RefreshToken);
                        //await _authService.LoginWithCookieAsync(response.Email, response.RefreshToken);

                        // HACK
                        // Save to custom session service
                        //_userSessionService.SetUserSession(response.Email, response.AccessToken, response.RefreshToken);
                        await Console.Out.WriteLineAsync("\n\n---login----- Setting UserSession ----------\n\n");
                        await _userSessionService.SetUserSessionAsync(response.Email, response.AccessToken, response.RefreshToken, response.Roles);
                        
                        await Console.Out.WriteLineAsync("\n\n---  END ----- Setting UserSession ----------\n\n");
                        await Console.Out.WriteLineAsync("---isLoggedIn -                           ---\n");
                        await Console.Out.WriteLineAsync($"\t\t{_userSessionService.IsLoggedIn}\n");
                        await Console.Out.WriteLineAsync("\n\n---  END ----- Setting UserSession ----------\n\n");
                        

                    }
                    catch (Exception ex)
                    {
                        await Console.Out.WriteLineAsync($"\n\n\n{ex}\n\n\n");
                        throw;
                    }
                }
                else
                {

                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(loginRequest);
                }

                //return Ok("Login successful");

                //once login is done
                //return RedirectToAction("Index", "Auth");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //remove stored tokens from database by calling web api
            await _authService.RevokeTokenAsync();

            //log the user out by removing cookie from mvc app
            await _authService.LogoutAsync();

            await _userSessionService.CloseUserSessionData();

            //redirect to home page once logged out
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Any time we need to refresh token, we can redirect to this endpoint. What the refresh endpoint will do is to get the new set of access and refresh tokens from the Web API, and login the user with these tokens, basically rewriting the cookie with new claims
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Refresh(string returnUrl)
        {
            var response = await _authService.RefreshTokenAsync();
            if (response.Status == HttpStatusCode.OK)
            {
                await _authService.LoginWithCookieAsync(response.Email, response.AccessToken, response.RefreshToken); //the cookie is created only when the control is returned to the browser https://memorycrypt.hashnode.dev/net-mvc-app-calling-web-api-for-authentication
                return RedirectTo(returnUrl);
            }
            else
            {
                //if refresh request failed, redirect to login page
                return RedirectToAction("login");
            }
        }

        private IActionResult RedirectTo(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                //default redirect
                return RedirectToAction("Index", "Home");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
