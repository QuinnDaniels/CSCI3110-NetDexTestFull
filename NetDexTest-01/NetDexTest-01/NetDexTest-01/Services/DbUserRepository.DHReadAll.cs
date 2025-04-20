using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;

namespace NetDexTest_01.Services
{
    // assisted partially with chatGPT
    public partial class DbUserRepository : IUserRepository
    {
        public async Task<ICollection<DexHolder>> ReadAllDexHolders()
        {
            return await _db.DexHolder.ToListAsync();
        }
        public async Task<ICollection<DexHolder>> ReadAllDexAsync()
        {
            return await _db.DexHolder.ToListAsync(); // I/O Bound Operation
        }

        
    }
}
