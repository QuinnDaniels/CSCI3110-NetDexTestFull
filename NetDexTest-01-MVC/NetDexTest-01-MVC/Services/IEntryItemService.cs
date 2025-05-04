using NetDexTest_01_MVC.Models.ViewModels;

namespace NetDexTest_01_MVC.Services
{
    public interface IEntryItemService
    {
        Task<List<EntryItemVM>> GetAllAsync();
        Task<EntryItemVM> GetByIdAsync(long id);
        Task<bool> CreateAsync(EntryItemVM model);
        Task<bool> UpdateAsync(EntryItemVM model);
        Task<bool> DeleteAsync(long id);
    }
}
