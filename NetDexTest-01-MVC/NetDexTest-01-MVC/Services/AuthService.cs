using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Identity.Data;
using NetDexTest_01_MVC.Models;
using NetDexTest_01_MVC.Models.Authentication;
using System.Net.Http;
using NetDexTest_01_MVC.Models.ViewModels;


namespace NetDexTest_01_MVC.Services
{
    /// <summary>
    /// <seealso href="https://memorycrypt.hashnode.dev/net-mvc-app-calling-web-api-for-authentication#heading-2-add-web-api-urls-in-appsettingsjson">
    /// </summary>
    public class AuthService : IAuthService
    {
        private IApiCallerService _apiService;
        private IHttpContextAccessor _contextAccessor;
        private IConfiguration _config;
        private readonly IUserSessionService _userSessionService;
        //private IHttpClientFactory _httpClientFactory;
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="apiService">The api service.</param>
        /// <param name="contextAccessor">The context accessor.</param>
        public AuthService(//IHttpClientFactory  httpClientFactory,
            IConfiguration config, IApiCallerService apiService, IHttpContextAccessor contextAccessor,
            IUserSessionService userSessionService)
        {
            _config = config;
            _apiService = apiService;
            _contextAccessor = contextAccessor;
            _userSessionService = userSessionService;
            //    _httpClientFactory = httpClientFactory;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequestModel request)
        {
            //Code to call the Web Api
            var url = _config["apiService:userRegisterUrl"];
            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Post,
                url: url,
                bodyContent: request);

            var response = new RegisterResponse
            {
                Status = httpResponse.StatusCode
            };

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                //add error message
                response.Message = await httpResponse.Content.ReadAsStringAsync();
            }

            return response;
        }

        public async Task<LoginResponse> LoginAsyncOld(LoginRequest request)
        {
            var url = _config["apiService:userLoginUrl"];
            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Post,
                url: url,
                bodyContent: request
                );
            LoginResponse loginResponse = new LoginResponse();

            //if login was successful
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                //map the login response
                loginResponse = await httpResponse.Content.ReadFromJsonAsync<LoginResponse>();  //FIXME - only populates RefreshToken
                loginResponse.Status = httpResponse.StatusCode;
                //loginResponse.Token = httpResponse.StatusCode;
                loginResponse.Email = request.Email;
            }
            else
            {
                //else if login failed, map the error message
                var errMessage = await httpResponse.Content.ReadAsStringAsync();
                loginResponse.Status = httpResponse.StatusCode;
                loginResponse.Message = errMessage;
            }
            return loginResponse;
        }


        public async Task LoginWithCookieAsync(string email, string accessToken, string refreshToken)
        {
            //generate claims for email, access token, and refresh token
            var principal = GeneratePrincipal(email, accessToken, refreshToken);

            //create a cookie with above claims
            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                });
        }
        public async Task LoginWithCookieAsync(string email, string refreshToken)
        {
            //generate claims for email, access token, and refresh token
            var principal = GeneratePrincipal(email, refreshToken);

            //create a cookie with above claims
            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        private ClaimsPrincipal GeneratePrincipal(string email, string accessToken, string refreshToken)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Email, email));
            identity.AddClaim(new Claim("token", accessToken));
            identity.AddClaim(new Claim("refresh", refreshToken));

            var principal = new ClaimsPrincipal(identity);
            return principal;
        }

        private ClaimsPrincipal GeneratePrincipal(string email, string refreshToken)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Email, email));
            //identity.AddClaim(new Claim("token", accessToken));
            identity.AddClaim(new Claim("refresh", refreshToken));

            var principal = new ClaimsPrincipal(identity);
            return principal;
        }


        public async Task<bool> RevokeTokenAsync()
        {
            var url = _config["apiService:tokenRevokeUrl"];

            var currentClaims = GetSavedClaims();

            if (!(string.IsNullOrEmpty(currentClaims.AuthToken) || string.IsNullOrEmpty(currentClaims.RefreshToken)))
            {
                var revokeRequest = new RevokeRequestModel
                {
                    RefreshToken = currentClaims.RefreshToken
                };

                var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Post,
                url: url,
                bodyContent: revokeRequest,
                authScheme: "bearer",
                authToken: currentClaims.AuthToken);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
            }

            return false;
        }

        public Claims GetSavedClaims()
        {
            var claimsList = _contextAccessor.HttpContext.User.Claims;
            var claimsObject = GenerateClaimsObject(claimsList);
            return claimsObject;
        }

        private Claims GenerateClaimsObject(IEnumerable<Claim> claims)
        {
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var token = claims.FirstOrDefault(c => c.Type == "token")?.Value;
            var refreshToken = claims.FirstOrDefault(c => c.Type == "refresh")?.Value;
            return new Claims { Email = email, AuthToken = token, RefreshToken = refreshToken };
        }

        public async Task LogoutAsync()
        {
            await _userSessionService.CloseUserSessionData();
            await _contextAccessor.HttpContext.SignOutAsync();
        }

        public async Task<LoginResponse> RefreshTokenAsync(Claims claims = null)
        {
            var url = _config["apiService:tokenRefreshUrl"];

            var currentClaims = claims == null ? GetSavedClaims() : claims;

            LoginResponse response = null;
            if (!(string.IsNullOrEmpty(currentClaims.AuthToken) || string.IsNullOrEmpty(currentClaims.RefreshToken)))
            {
                var refreshRequest = new RefreshRequestModel
                {
                    AccessToken = currentClaims.AuthToken,
                    RefreshToken = currentClaims.RefreshToken
                };

                //make a call to web api to get new tokens
                var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Post,
                url: url,
                bodyContent: refreshRequest);

                //if call was successful
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    response = await httpResponse.Content.ReadFromJsonAsync<LoginResponse>();
                    response.Status = httpResponse.StatusCode;
                    response.Email = currentClaims.Email;
                }
                else
                {
                    //if call failed
                    response = new LoginResponse
                    {
                        Status = httpResponse.StatusCode,
                        Message = await httpResponse.Content.ReadAsStringAsync()
                    };
                }
            }
            return response;
        }

        /*-------------------------------*/


        public async Task TakeActionIfTokenExpired(CookieValidatePrincipalContext context)
        {
            //get current claims from CookieValidatePrincipalContext
            var currentClaims = GenerateClaimsObject(context.Principal.Claims); //can't fetch from user claims yet as they are not saved
            var shouldRedirectToLogin = false;

            if (string.IsNullOrEmpty(currentClaims.AuthToken))
            {
                shouldRedirectToLogin = true;
            }
            else
            {
                //check if auth token is still valid
                var isTokenValid = await ValidateToken(currentClaims.AuthToken);
                if (!isTokenValid)
                {
                    //refresh auth token 
                    var refreshResponse = await RefreshTokenAsync(currentClaims);
                    if (refreshResponse.Status != HttpStatusCode.OK)
                    {
                        shouldRedirectToLogin = true;
                    }
                    else
                    {
                        //update the principal (claims) in context object
                        var newPrincipal = GeneratePrincipal(refreshResponse.Email, refreshResponse.AccessToken, refreshResponse.RefreshToken);
                        context.ReplacePrincipal(newPrincipal);
                        context.ShouldRenew = true;
                    }
                } // if token is valid, exit the function
            }

            if (shouldRedirectToLogin)
            {
                //if for any reason tokens could not be refreshed
                //reject the principal and remove the current cookie  
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync();
            }
        }

        private async Task<bool> ValidateToken(string authToken)
        {
            var url = _config["apiService:tokenValidateUrl"];

            if (!(string.IsNullOrEmpty(authToken)))
            {
                var httpResponse = await _apiService.MakeHttpCallAsync(
                        httpMethod: HttpMethod.Post,
                        url: url,
                        authScheme: "bearer",
                        authToken: authToken);

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
            }

            return false;
        }





        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            //var client = _httpClientFactory.CreateClient("API");
            //var response = await client.PostAsJsonAsync("auth/login", request);

            var url = _config["apiService:userLoginUrl"];
            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Post,
                url: url,
                bodyContent: request
                );
            LoginResponse? loginResponse = new LoginResponse();

            await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----is success?--\n\n{httpResponse.IsSuccessStatusCode}\n\n\n-----------------------");
            if (httpResponse.IsSuccessStatusCode)
            {
                loginResponse = await httpResponse.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse == null)
                {
                    await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\nNULL!!\n\n\n-----------------------");
                }
                else
                {
                    await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.ToString}\n\n\n-----------------------");
                    await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{await httpResponse.Content.ReadAsStringAsync()}\n\n\n-----------------------");
                }
            }

            else
            {
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----is success?--\n\n{httpResponse.IsSuccessStatusCode}\n\n\n-----------------------");
                //else if login failed, map the error message
                var errMessage = await httpResponse.Content.ReadAsStringAsync();
                loginResponse.Status = httpResponse.StatusCode;
                loginResponse.Message = errMessage;
            }
            return loginResponse;
        }

        public async Task<LoginResponse> LoginAsyncAlternative(LoginRequest request)
        {
            //var client = _httpClientFactory.CreateClient("API");
            //var response = await client.PostAsJsonAsync("auth/login", request);

            var url = _config["apiService:userLogin2Url"];
            //var url = _config["apiService:userLogin2Url"];
            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Post,
                url: url,
                bodyContent: request
                );
            LoginResponse loginResponse = new LoginResponse();

            await Console.Out.WriteLineAsync($"\n\n\n--------LoginRequest------email-------\n\n{request.Email}\n\n\n-----------------------");
            await Console.Out.WriteLineAsync($"\n\n\n--------LoginRequest------username-------\n\n{request.Username}\n\n\n-----------------------");
            await Console.Out.WriteLineAsync($"\n\n\n--------LoginRequest------password----\n\n{request.Password}\n\n\n-----------------------");
            await Console.Out.WriteLineAsync($"\n\n\n--------httpResponse------------------\n\n{httpResponse.Content.ReadAsStringAsync()}\n\n\n-----------------------");
            //await Console.Out.WriteLineAsync($"\n\n\n--------httpResponse-----json---------\n\n{httpResponse.Content.ReadFromJsonAsync<LoginResponse>()}\n\n\n-----------------------");

            await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----is success?--\n\n{httpResponse.IsSuccessStatusCode}\n\n\n-----------------------");

            await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----code?--\n\n{httpResponse.StatusCode}\n\n\n-----------------------");

            await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----message?--\n\n{httpResponse.RequestMessage}\n\n\n-----------------------");
            await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----reasonPhrase?--\n\n{httpResponse.ReasonPhrase}\n\n\n-----------------------");
            await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----headers?--\n\n{httpResponse.Headers}\n\n\n-----------------------");



            if (httpResponse.IsSuccessStatusCode)
            {
                loginResponse = await httpResponse.Content.ReadFromJsonAsync<LoginResponse>();
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.ToString}\n\n\n-----------------------");
                //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{await httpResponse.Content.ReadAsStringAsync()}\n\n\n-----------------------");
                await _userSessionService.StorePasswordSessionDataAsync(request.Password);
                await _userSessionService.StoreCurrentUserDataAsync(loginResponse.Username);

            }

            else
            {
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----is success?--\n\n{httpResponse.IsSuccessStatusCode}\n\n\n-----------------------");
                //else if login failed, map the error message
                var errMessage = await httpResponse.Content.ReadAsStringAsync();
                loginResponse.Status = httpResponse.StatusCode;
                loginResponse.Message = errMessage;
            }
            return loginResponse;
        }


        public async Task<bool> CheckInputAgainstSessionAsync(string id)
        {
            string currUsername = _userSessionService.GetUsername();
            string currEmail = _userSessionService.GetEmail();
            string currPass = _userSessionService.GetPass();
            await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession--- currPass -----\n\t{currPass}");
            await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession--- currName ------\n\t{currUsername}");
            await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession--- currEmail -----\n\t{currEmail}");
            bool flagSetter = false;
            bool flagSkipper = false;
            await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession--- flagSetter ----\n\t{flagSetter}");
            await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession--- flagSkipper ---\n\t{flagSkipper}");

            LoginRequest verification = new LoginRequest()
            {
                Password = currPass
            };
            await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession--- id ------------\n\t{id}");

            if (id == currUsername)
            {
                await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession-------------------\n\tid, {id}, matches Username!");
                verification!.Email = currEmail;
                verification!.Username = id;
            }
            else if (id == currEmail)
            {
                await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession-------------------\n\tid, {id}, matches Email!");
                verification!.Email = id;
                verification!.Username = currUsername;
            }
            else
            {
                await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession-------------------\n\tid matched neither Email nor Username!");
                flagSkipper = true;
                await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession--- flagSkipper ---\n\t{flagSkipper}");
            }


            if (!flagSkipper)
            {

                await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession----- request --\n\t{verification.Email}\n\t{verification.Username}\n\t{verification.Input}\n\t{verification.Password}");
                var url = _config["apiService:userCheckUrl"];
                await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession---- url -------\n");
                await Console.Out.WriteLineAsync($"\n\tURL: {url}");

                var httpResponse = await _apiService.MakeHttpCallAsync(
                    httpMethod: HttpMethod.Post,
                    url: url,
                    bodyContent: verification // HERE ?
                    );
                LoginResponse loginResponse = new LoginResponse();

                await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession---- code -------\n");
                await Console.Out.WriteLineAsync($"\n\n\t\t{httpResponse.StatusCode}\n");
                await Console.Out.WriteLineAsync($"\n\n-------------------------------------------------\n");
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    loginResponse = await httpResponse.Content.ReadFromJsonAsync<LoginResponse>();

                    if (loginResponse != null && loginResponse.Username == currUsername && loginResponse.Email == currEmail)
                    {
                        flagSetter = true;
                        await Console.Out.WriteLineAsync($"\n\tflagSetter: {flagSetter}");

                    }
                }

                else
                {
                    await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----is success?--\n\n{httpResponse.IsSuccessStatusCode}\n\n\n-----------------------");
                    //else if login failed, map the error message
                    var errMessage = await httpResponse.Content.ReadAsStringAsync();
                    loginResponse.Status = httpResponse.StatusCode;
                    loginResponse.Message = errMessage;
                    await Console.Out.WriteLineAsync($"{loginResponse.Status}");
                    await Console.Out.WriteLineAsync($"{loginResponse.Message}");
                }
            }
            else
            {
                await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession---- FLAGSKIPPER --------\n");

            }

            await Console.Out.WriteLineAsync($"\n\n--------CheckInputAgainstSession---- END --------\n");
            return flagSetter;

        }


        public async Task<bool> CheckIfUserExistsAsync(string id)
        {
            //var client = _httpClientFactory.CreateClient("API");
            //var response = await client.PostAsJsonAsync("auth/login", request);
            await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------START--------------\n\t");

            bool flagCheck = false;
            await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------flagCheck?---init---\n\t{flagCheck}");

            var url = _config["apiService:userUrl"];
            url = url + $"/one/{id}";

            await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------url target---init---\n\t{url}");

            //LoginRequest request = new()
            //{
            //    Password = _userSessionService.GetPass(),

            //};


            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Get,
                url: url
                //bodyContent: request
                );
            DexHolderMiddleVMResponse loginResponse = new();

            await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{httpResponse.StatusCode}\n\n\n-----------------------");

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                await Console.Out.WriteLineAsync("StatusCode is OK!\n");
                loginResponse = await httpResponse.Content.ReadFromJsonAsync<DexHolderMiddleVMResponse>();
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{httpResponse.ToString}\n\n\n-----------------------");
                //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Status}\n\n\n-----------------------");
                //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{await httpResponse.Content.ReadAsStringAsync()}\n\n\n-----------------------");
                await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------setting tempCheck---\n\t...");
                await _userSessionService.SetTempCheckAsync(loginResponse.ApplicationEmail);
                flagCheck = true;
                await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------flagCheck----set----\n\t{flagCheck}");

            }

            else
            {
                //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Message}\n\n\n-----------------------");
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----is success?--\n\n{httpResponse.IsSuccessStatusCode}\n\n\n-----------------------");
                //else if login failed, map the error message
                //var errMessage = await httpResponse.Content.ReadAsStringAsync();
                //loginResponse.Status = httpResponse.StatusCode;
                //loginResponse.Message = errMessage;
                //await Console.Out.WriteLineAsync($"\n\n\\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Status}\n\n\n-----------------------");
                //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Title}\n\n\n-----------------------");
                //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Message}\n\n\n-----------------------");
            }

            await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------flagCheck---RESULT--\n\t{flagCheck}");
            await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists----END--------------------\n");
            return flagCheck;
        }



        public async Task<bool> CheckIfUserExistsAsync(string id, string criteria)
        {
            //var client = _httpClientFactory.CreateClient("API");
            //var response = await client.PostAsJsonAsync("auth/login", request);
            await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------START--------------\n\t");

            bool flagCheck = false;
            await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------flagCheck?---init---\n\t{flagCheck}");

            var url = _config["apiService:userUrl"];
            url = url + $"/dex/{id}";

            await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------url target---init---\n\t{url}");

            //LoginRequest request = new()
            //{
            //    Password = _userSessionService.GetPass(),

            //};


            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Get,
                url: url
                //bodyContent: request
                );
            DexHolderMiddleVMResponse loginResponse = new();

            await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{httpResponse.StatusCode}\n\n\n-----------------------");

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                await Console.Out.WriteLineAsync("StatusCode is OK!\n");
                loginResponse = await httpResponse.Content.ReadFromJsonAsync<DexHolderMiddleVMResponse>();
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{httpResponse.ToString}\n\n\n-----------------------");
                //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Status}\n\n\n-----------------------");
                //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{await httpResponse.Content.ReadAsStringAsync()}\n\n\n-----------------------");
                await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------setting tempCheck---\n\t...");

                var peopleList = loginResponse.People.OrderBy(p => p.LocalCounter).ToList();
                await Console.Out.WriteLineAsync($"\n\n--LIST--PeopleList for {id}.... ----------\n\t...");
                foreach (var person in peopleList)
                {
                await Console.Out.WriteAsync($"\n\t> {person.Id} - {person.LocalCounter} - {person.Nickname}...");

                }
                PersonDexListVM? personSelect = null;
                bool flagCheckInt = false;
                try
                {
                    await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------Attempting parse!----\n\t{flagCheck}");
                    if (int.TryParse(criteria, out int idout)) // in case you want to try to use the local counter
                    {
                        await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------criteria parsed as int ----\n\tidout: {idout}, criteria: {criteria}");
                        personSelect = peopleList.FirstOrDefault(p => p.LocalCounter == idout);
                        flagCheckInt = true;
                    }
                    else { await Console.Out.WriteLineAsync($"\n\n\nINFO: Person was not able to be found in DexHolder People list using criteria as LocalCounter, {criteria}!!!\n\n\n"); }
                    if (personSelect == null)
                    {
                        await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------Checking nickname!----\n\t{flagCheck}");
                        personSelect = peopleList.FirstOrDefault(p => p.Nickname == criteria);
                    }

                    if (personSelect != null)
                    {
                        await Console.Out.WriteLineAsync($"\n\n\nLOG:\n\n\t{personSelect.Id} - {personSelect.LocalCounter} - {personSelect.Nickname}\n\n\n");
                        if (flagCheckInt == true)
                        {
                            // NOTE: momentarily thought id need to use idout
                            await _userSessionService.SetTempCheckAsync(loginResponse.ApplicationEmail, criteria);
                            flagCheck = true;
                            await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------flagCheck----set----\n\t{flagCheck}");
                        }
                        else
                        {
                            await _userSessionService.SetTempCheckAsync(loginResponse.ApplicationEmail, criteria);
                            flagCheck = true;
                            await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------flagCheck----set----\n\t{flagCheck}");
                        }
                        //return personSelect;
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync($"\n\n\nWARNING: user was not able to be found in DexHolder People list using criteria, {id}!!!\n\n\n");
                    }
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync($"\n\n\nERROR: {ex}!!!\n\n\n");
                }
             }



            else
                {
                    //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Message}\n\n\n-----------------------");
                    await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----is success?--\n\n{httpResponse.IsSuccessStatusCode}\n\n\n-----------------------");
                    //else if login failed, map the error message
                    //var errMessage = await httpResponse.Content.ReadAsStringAsync();
                    //loginResponse.Status = httpResponse.StatusCode;
                    //loginResponse.Message = errMessage;
                    //await Console.Out.WriteLineAsync($"\n\n\\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Status}\n\n\n-----------------------");
                    //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Title}\n\n\n-----------------------");
                    //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Message}\n\n\n-----------------------");
                }

                await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------flagCheck---RESULT--\n\t{flagCheck}");
                await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists----END--------------------\n");
                return flagCheck;
            }




            public async Task<DexHolderMiddleVM?> CheckIfUserExistsReturnObjectAsync(string id)
            {
                //var client = _httpClientFactory.CreateClient("API");
                //var response = await client.PostAsJsonAsync("auth/login", request);
                await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------START--------------\n\t");

                bool flagCheck = false;
                await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------flagCheck?---init---\n\t{flagCheck}");

                var url = _config["apiService:userUrl"];
                url = url + $"/one/{id}";

                await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------url target---init---\n\t{url}");

                //LoginRequest request = new()
                //{
                //    Password = _userSessionService.GetPass(),

                //};


                var httpResponse = await _apiService.MakeHttpCallAsync(
                    httpMethod: HttpMethod.Get,
                    url: url
                    //bodyContent: request
                    );
                DexHolderMiddleVMResponse loginResponse = new();
                DexHolderMiddleVM? outDex = null;
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{httpResponse.StatusCode}\n\n\n-----------------------");

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    await Console.Out.WriteLineAsync("StatusCode is OK!\n");
                    loginResponse = await httpResponse.Content.ReadFromJsonAsync<DexHolderMiddleVMResponse>();
                    await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{httpResponse.ToString}\n\n\n-----------------------");
                    //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Status}\n\n\n-----------------------");
                    //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{await httpResponse.Content.ReadAsStringAsync()}\n\n\n-----------------------");
                    await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------setting tempCheck---\n\t...");
                    await _userSessionService.SetTempCheckAsync(loginResponse.ApplicationEmail);
                    flagCheck = true;
                    outDex = loginResponse.getInstanceDexHolderMiddleVM();
                    await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------flagCheck----set----\n\t{flagCheck}");

                }

                else
                {
                    //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Message}\n\n\n-----------------------");
                    await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----is success?--\n\n{httpResponse.IsSuccessStatusCode}\n\n\n-----------------------");
                    //else if login failed, map the error message
                    //var errMessage = await httpResponse.Content.ReadAsStringAsync();
                    //loginResponse.Status = httpResponse.StatusCode;
                    //loginResponse.Message = errMessage;
                    //await Console.Out.WriteLineAsync($"\n\n\\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Status}\n\n\n-----------------------");
                    //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Title}\n\n\n-----------------------");
                    //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{loginResponse.Message}\n\n\n-----------------------");
                }

                await Console.Out.WriteLineAsync($"\n\n--------CheckIfUserExists-------flagCheck---RESULT--\n\t{flagCheck}");
                await Console.Out.WriteLineAsync($"\n\n-------CheckIfUserExists----END--returning object---\n");
                //return flagCheck;
                return outDex;
            }






        }

    } 