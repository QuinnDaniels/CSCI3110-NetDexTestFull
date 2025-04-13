using NetDexTest_01.Models.Entities;

namespace NetDexTest_01.Services
{
    public partial interface IUserRepository
    {

        Task<DexHolder?> GetDexHolderByUserIdAsync(string userId);
        Task<DexHolder> GetDexHolderByUserNameAsync(string userName);
        Task AddDexHolderAsync(DexHolder userDetails);
        Task SaveChangesAsync();

    }
}
