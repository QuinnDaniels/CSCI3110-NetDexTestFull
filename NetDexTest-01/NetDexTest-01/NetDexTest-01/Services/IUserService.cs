using NetDexTest_01.Models;
using NetDexTest_01.Models.Entities;
using System.Security.Claims;

namespace NetDexTest_01.Services
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterModel model);

        Task<ApplicationUser?> CreateRegisterAsync(RegisterModel model);

        Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);

        Task<string> AddRoleAsync(AddRoleModel model);

        
    }
}
