using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;

namespace NetDexTest_01.Services
{
    // assisted partially with chatGPT
    public partial class DbUserRepository : IUserRepository
    {
        public async Task DeleteAsync(PropertyField pType, string input)
        {
            switch (pType)
            {
                case PropertyField.id:
                    await DeleteByIdAsync(input);
                    break;
                case PropertyField.username:
                    await DeleteByUsernameAsync(input);
                    break;
                default:
                    throw new InvalidOperationException("PropertyField is invalid! Check the switch-case and try again");

            }
        }

        public async Task DeleteByIdAsync(string id)
        {
            var dexHolderToDelete = await ReadDexByIdAsync(id);
            var userToDelete = await ReadByIdAsync(id);
            if (dexHolderToDelete != null)
            {
                _db.DexHolder.Remove(dexHolderToDelete);
                await _db.SaveChangesAsync();
            }
            if (userToDelete != null)
            {
                _db.Users.Remove(userToDelete);
                await _db.SaveChangesAsync();

            }
        }
        public async Task DeleteByUsernameAsync(string username)
        {
            var dexHolderToDelete = await ReadDexByUsernameAsync(username);
            var userToDelete = await ReadByUsernameAsync(username);

            try
            {
                if (userToDelete != null)
                {
                    _db.Users.Remove(userToDelete);
                    await _db.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex}");
            }

            try
            {
                if (dexHolderToDelete != null)
                {
                    _db.DexHolder.Remove(dexHolderToDelete);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex}");
            }
        }

    }
}
