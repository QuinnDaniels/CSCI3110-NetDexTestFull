using NetDexTest_01_MVC.Models.ViewModels;

namespace NetDexTest_01_MVC.Services
{
    public interface IUserService
    {
        Task<ICollection<AdminUserVM>?> GetAllUsersAdminAsync();
        //Task<ICollection<AdminUserVMResponse>> Authorized_GetAllUsersAdminAsync();
    }
}
