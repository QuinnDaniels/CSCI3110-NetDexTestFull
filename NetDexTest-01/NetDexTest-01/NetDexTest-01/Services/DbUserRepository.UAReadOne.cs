using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;

namespace NetDexTest_01.Services
{
    // assisted partially with chatGPT
    public partial class DbUserRepository : IUserRepository
    {
        public async Task<ApplicationUser?> GetByEmailAsync(string email, string password)
        {
            var conversion = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            var t = await _userManager.CheckPasswordAsync((ApplicationUser?)conversion, password);
            if (t == true) return (ApplicationUser?)conversion;
            else return null;
        }
        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            var conversion = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            return (ApplicationUser?)conversion;
        }
        public async Task<ApplicationUser?> GetByUsernameAsync(string username, string password)
        {
            var conversion = await _db.Users.FirstOrDefaultAsync(u => u.UserName == username);
            var t = await _userManager.CheckPasswordAsync((ApplicationUser?)conversion, password);
            if (t == true) return (ApplicationUser?)conversion;
            else return null;
        }
        public async Task<ApplicationUser?> GetByUsernameAsync(string username)
        {
            var conversion = await _db.Users.FirstOrDefaultAsync(u => u.UserName == username);
            return (ApplicationUser?)conversion;
        }
        public async Task<ApplicationUser?> GetByIdAsync(string id, string password)
        {
            var conversion = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            var t = await _userManager.CheckPasswordAsync((ApplicationUser?)conversion, password);
            if (t == true) return (ApplicationUser?)conversion;
            else return null;
        }
        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            var conversion = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            return (ApplicationUser?)conversion;
        }

        public async Task<ApplicationUser?> GetUserAsync(PropertyField pType, string input)
        {
            ApplicationUser? user = null;
            switch (pType)
            {
                case PropertyField.id:
                    user = await GetByIdAsync(input);
                    break;
                case PropertyField.username:
                    user = await GetByUsernameAsync(input);
                    break;
                case PropertyField.email:
                    user = await GetByEmailAsync(input);
                    break;
                default:
                    throw new InvalidOperationException("PropertyField is invalid! Check the switch-case and try again");
            }
            return user;
        }


        public async Task<ApplicationUser?> GetUserAsync(PropertyField pType, string input, string password)
        {
            ApplicationUser? user = null;
            switch (pType)
            {
                case PropertyField.id:
                    user = await GetByIdAsync(input, password);
                    break;
                case PropertyField.username:
                    user = await GetByUsernameAsync(input, password);
                    break;
                case PropertyField.email:
                    user = await GetByEmailAsync(input, password);
                    break;
                default:
                    throw new InvalidOperationException("PropertyField is invalid! Check the switch-case and try again");
            }
            return user;
        }





        public async Task<ApplicationUser?> ReadByUsernameAsync(string username)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user != null)
            {
                user.Roles = await _userManager.GetRolesAsync(user);
            }
            return user;
        }
        public async Task<ApplicationUser?> ReadByIdAsync(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                user.Roles = await _userManager.GetRolesAsync(user);
            }
            return user;
        }
        public async Task<ApplicationUser?> ReadUserAsync(PropertyField pType, string input)
        {
            ApplicationUser? user = null;
            switch (pType)
            {
                case PropertyField.id:
                    user = await ReadByIdAsync(input);
                    break;
                case PropertyField.username:
                    user = await ReadByUsernameAsync(input);
                    break;
                default:
                    throw new InvalidOperationException("PropertyField is invalid! Check the switch-case and try again");
            }
            return user;
        }


    }
}
