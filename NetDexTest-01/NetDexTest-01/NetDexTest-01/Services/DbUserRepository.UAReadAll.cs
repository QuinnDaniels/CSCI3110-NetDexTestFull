using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;

namespace NetDexTest_01.Services
{
    // assisted partially with chatGPT
    public partial class DbUserRepository : IUserRepository
    {
        public async Task<ICollection<ApplicationUser>> ReadAllApplicationUsers()
        {
            return await _db.Users
                .Include(u => u.DexHolder)
                .ToListAsync();
        }

        public async Task<ICollection<ApplicationUser>> ReadAllUserDexPeopleAsync()
        {
            return await _db.Users
                .Include(u => u.DexHolder)
                    .ThenInclude(dh => dh.People)
                .ToListAsync();
        }

    }
}
