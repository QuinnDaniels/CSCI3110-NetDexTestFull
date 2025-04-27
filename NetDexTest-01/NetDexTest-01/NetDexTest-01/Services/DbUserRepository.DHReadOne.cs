using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;

namespace NetDexTest_01.Services
{
    // assisted partially with chatGPT
    public partial class DbUserRepository : IUserRepository
    {
        #region ReadDex
        public async Task<DexHolder?> ReadDexAsync(PropertyField pType, string input)
        {
            switch (pType)
            {
                case PropertyField.id:
                    return await ReadDexByIdAsync(input);
                    break;
                case PropertyField.username:
                    return await ReadDexByUsernameAsync(input);
                    break;
                case PropertyField.email:
                    return await GetDexHolderByEmailAsync(input);
                    break;
                default:
                    throw new InvalidOperationException("PropertyField is invalid! Check the switch-case and try again");
            }
            //return await _db.People.FindAsync(id);
            //return await _db.DexHolder.FirstOrDefaultAsync(p => p.ApplicationUserId == input); //Takes a lamda expression as its parameter. Slightly slower than first return option but slightly more flexible
        }
        public async Task<DexHolder?> ReadDexByIdAsync(string applicationUserId)
        {
            //return await _db.People.FindAsync(id);
            return await _db.DexHolder.FirstOrDefaultAsync(p => p.ApplicationUserId == applicationUserId); //Takes a lamda expression as its parameter. Slightly slower than first return option but slightly more flexible
        }
        public async Task<DexHolder?> ReadDexByIdAsync(int dexHolderId)
        {
            //return await _db.People.FindAsync(id);
            return await _db.DexHolder.FirstOrDefaultAsync(dh => dh.Id == dexHolderId); //Takes a lamda expression as its parameter. Slightly slower than first return option but slightly more flexible
        }

        public async Task<DexHolder?> ReadDexByUsernameAsync(string username)
        {
            //return await _db.People.FindAsync(id);
            return await _db.DexHolder.FirstOrDefaultAsync(p => p.ApplicationUserName == username); //Takes a lamda expression as its parameter. Slightly slower than first return option but slightly more flexible
        }
        #endregion ReadDex

        //moved from .GPT
        public async Task<DexHolder?> GetDexHolderByUserIdAsync(string userId)
        {
            //try
            //{
            return await _db.DexHolder
                .Include(dh => dh.ApplicationUser)
                .Include(dh => dh.People)
                .ThenInclude(p => p.FullName)
                .Include(dh => dh.People)
                .ThenInclude(p => p.ContactInfo)
                .ThenInclude(ci => ci.SocialMedias)
                .Include(dh => dh.People)
                .ThenInclude(p => p.RecordCollector)
                .ThenInclude(rc => rc.EntryItems)
                .Include(dh => dh.People)
                .ThenInclude(p => p.PersonChildren)
                .Include(dh => dh.People)
                .ThenInclude(p => p.PersonParents)
                .FirstOrDefaultAsync(dh => dh.ApplicationUserId == userId);
            //}
            //catch (Exception ex) {
            //    Console.WriteLine($"---------------------\nERROR: {ex}\n---------------------");
            //    return null;
            //}
        }


        public async Task<DexHolder?> GetDexHolderByIntIdAsync(int id)
        {
            //try
            //{
            return await _db.DexHolder
                .Include(dh => dh.ApplicationUser)
                .Include(dh => dh.People)
                    .ThenInclude(p => p.FullName)
                .Include(dh => dh.People)
                    .ThenInclude(p => p.ContactInfo)
                        .ThenInclude(ci => ci.SocialMedias)
                .Include(dh => dh.People)
                    .ThenInclude(p => p.RecordCollector)
                        .ThenInclude(rc => rc.EntryItems)
                .Include(dh => dh.People)
                    .ThenInclude(p => p.PersonChildren)
                .Include(dh => dh.People)
                    .ThenInclude(p => p.PersonParents)
                .FirstOrDefaultAsync(dh => dh.Id == id);
            //}
            //catch (Exception ex) {
            //    Console.WriteLine($"---------------------\nERROR: {ex}\n---------------------");
            //    return null;
            //}
        }


        //moved from .GPT
        public async Task<DexHolder?> GetDexHolderByUserNameAsync(string userName)
        {
            return await _db.DexHolder
                .Include(dh => dh.ApplicationUser)
                .Include(dh => dh.People)
                .ThenInclude(p => p.FullName)
                .Include(dh => dh.People)
                .ThenInclude(p => p.ContactInfo)
                .ThenInclude(ci => ci.SocialMedias)
                .Include(dh => dh.People)
                .ThenInclude(p => p.RecordCollector)
                .ThenInclude(rc => rc.EntryItems)
                .Include(dh => dh.People)
                .ThenInclude(p => p.PersonChildren)
                .Include(dh => dh.People)
                .ThenInclude(p => p.PersonParents)
                .FirstOrDefaultAsync(dh => dh.ApplicationUserName == userName);
        }

        //moved from .GPT
        public async Task<DexHolder?> GetDexHolderByEmailAsync(string email)
        {
            var ufind = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (ufind != null)
            {
                return await _db.DexHolder
                    .Include(dh => dh.ApplicationUser)
                    .Include(dh => dh.People)
                    .ThenInclude(p => p.FullName)
                    .Include(dh => dh.People)
                    .ThenInclude(p => p.ContactInfo)
                    .ThenInclude(ci => ci.SocialMedias)
                    .Include(dh => dh.People)
                    .ThenInclude(p => p.RecordCollector)
                    .ThenInclude(rc => rc.EntryItems)
                    .Include(dh => dh.People)
                    .ThenInclude(p => p.PersonChildren)
                    .Include(dh => dh.People)
                    .ThenInclude(p => p.PersonParents)
                    .FirstOrDefaultAsync(dh => dh.ApplicationUserName == ufind.UserName);
            }
            else { return null; }

        }




    }
}
