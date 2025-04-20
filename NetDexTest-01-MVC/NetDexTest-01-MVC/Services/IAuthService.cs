using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetDexTest_01_MVC.Models;
using NuGet.Protocol.Plugins;


namespace NetDexTest_01_MVC.Services
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequestModel request);
        Task<LoginResponse> LoginAsync(LoginRequestModel request);
        Task LoginWithCookieAsync(string email, string accessToken, string refreshToken);
        Task<bool> RevokeTokenAsync();
        Task LogoutAsync();
        Task<LoginResponse> RefreshTokenAsync(Claims claims = null);
    }
}