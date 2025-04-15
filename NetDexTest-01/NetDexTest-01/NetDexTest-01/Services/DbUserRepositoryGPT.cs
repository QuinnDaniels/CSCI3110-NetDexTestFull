using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;

namespace NetDexTest_01.Services
{
    public partial class DbUserRepository : IUserRepository
    {
        
        //public async Task<DexHolder?> GetDexHolder<T>(); // isnt there a way to make a method that you feed a type into? Generics?

        public async Task<DexHolder?> GetDexHolderByUserIdAsync(string userId)
        {
            //try
            //{
            return await _db.DexHolder
                .Include(dh => dh.ApplicationUser)
                .FirstOrDefaultAsync(dh => dh.ApplicationUserId == userId);
            //}
            //catch (Exception ex) {
            //    Console.WriteLine($"---------------------\nERROR: {ex}\n---------------------");
            //    return null;
            //}
        }

        public async Task<DexHolder?> GetDexHolderByUserNameAsync(string userName)
        {
            return await _db.DexHolder
                .Include(dh => dh.ApplicationUser)
                .FirstOrDefaultAsync(dh => dh.ApplicationUserName == userName);
        }

        public async Task<DexHolder?> GetDexHolderByEmailAsync(string email)
        {
            var ufind = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (ufind != null)
            {
                return await _db.DexHolder
                    .Include(dh => dh.ApplicationUser)
                    .FirstOrDefaultAsync(dh => dh.ApplicationUserName == ufind.UserName);
            }
            else { return null; }

        }

        public async Task AddDexHolderAsync(DexHolder DexHolder)
        {
            await _db.DexHolder.AddAsync(DexHolder);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

    }
}
