using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using NetDexTest_01_MVC.Models;
using NetDexTest_01_MVC.Models.Authentication;
using NuGet.Protocol.Plugins;


namespace NetDexTest_01_MVC.Services
{
    /// <summary>
    /// <seealso href="https://memorycrypt.hashnode.dev/net-mvc-app-calling-web-api-for-authentication#heading-2-add-web-api-urls-in-appsettingsjson">
    /// </summary>
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequestModel request);
        Task<LoginResponse> LoginAsyncOld(LoginRequest request);
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<LoginResponse> LoginAsyncAlternative(LoginRequest request);
        Task LoginWithCookieAsync(string email, string accessToken, string refreshToken);
        Task LoginWithCookieAsync(string email, string refreshToken);
        Task<bool> RevokeTokenAsync();
        Task LogoutAsync();
        Task<LoginResponse> RefreshTokenAsync(Claims claims = null);
        Task TakeActionIfTokenExpired(CookieValidatePrincipalContext context);
        
        // FIXME
        //object GetSavedClaims();
        Claims GetSavedClaims();
    }
}