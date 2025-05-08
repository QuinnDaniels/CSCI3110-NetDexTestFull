using NetDexTest_01_MVC.Models.ViewModels;

namespace NetDexTest_01_MVC.Services
{
    public interface ISocialMediaService
    {
        Task<List<SocialMediaVM>> GetAllAsync();
        Task<SocialMediaVM?> GetByIdAsync(long id);
        Task<bool> CreateAsync(SocialMediaVM model);
        Task<bool> UpdateAsync(SocialMediaVM model);
        Task<bool> DeleteAsync(long id);
    }
}
