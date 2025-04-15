using NetDexTest_01.Models.Entities;

namespace NetDexTest_01.Services
{
    public partial interface IUserRepository
    {
        /// <summary>
        /// Find a DexHolder record 
        /// </summary>
        /// <remarks>using an ApplicationUser Id</remarks>
        /// <param name="userId"></param>
        /// <returns>DexHolder object, or null if not found</returns>
        Task<DexHolder?> GetDexHolderByUserIdAsync(string userId);
        /// <remarks>using an ApplicationUser UserName</remarks>
        /// <param name="userName"></param>
        /// <inheritdoc cref="GetDexHolderByUserIdAsync(string)"/>
        Task<DexHolder?> GetDexHolderByUserNameAsync(string userName);
        /// <remarks>using an ApplicationUser Email. Queries ApplicationUser table and uses the found User Username to find the DexHolder.</remarks>
        /// <param name="email">email found in ApplicationUser entity</param>
        /// <inheritdoc cref="GetDexHolderByUserIdAsync(string)"/>
        Task<DexHolder?> GetDexHolderByEmailAsync(string email);

        Task AddDexHolderAsync(DexHolder userDetails);
        Task SaveChangesAsync();

    }
}
