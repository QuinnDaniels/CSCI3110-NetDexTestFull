using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NetDexTest_01.Models.Entities;

namespace NetDexTest_01.Services
{
    // TODO: do this!
    /// <summary>
    /// interacts with Extensions of Person (Ci, Fn, Rc)
    /// </summary>
    public partial class DbPersonRepository
    {
        // read just a person

        public void GetPersonByNickName() { throw new NotImplementedException(); }
        /// <summary>
        /// A Helper Method that supplements <see cref="GetPersonByNickName()"/>
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        /// <inheritdoc cref="GetPersonByNickName()" />
        public void GetPersonByNickNameTool() { throw new NotImplementedException(); }


        /// <remarks>
        /// Uses an ApplicationUser object to access ApplicationUser.DexHolder directly to search for a person with the DexHolderId
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickName()" />
        public async Task<Person?> GetPersonByNickName(string nickName, ApplicationUser user)
        {
            return await GetPersonByNickNameWithUser(nickName, user);
        }
        /// <remarks>
        /// Uses a DexHolder object to access DexHolderId directly to search for a person with the DexHolderId
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickName()" />

        public async Task<Person?> GetPersonByNickName(string nickName, DexHolder dex)
        {
            return await GetPersonByNickNameWithDex(nickName, dex);
        }

        public async Task<Person?> GetPersonByNickName(PropertyField pType, string inputProperty, string nickName)
        {
            var person = new Person();
            switch (pType)
            {
                case PropertyField.id:
                    person = await GetPersonByNickNameWithUserIdAsync(inputProperty, nickName);
                    break;
                case PropertyField.username:
                    person = await GetPersonByNickNameWithUserNameAsync(inputProperty, nickName);
                    break;
                case PropertyField.email:
                    person = await GetPersonByNickNameWithEmailAsync(inputProperty, nickName);
                    break;
                default:
                    throw new ArgumentException();
                    break;
            }
            return person;
        }

        ///// <remarks> Used by: <code><seealso cref="GetPersonByNickName(string, ApplicationUser)"/></code> </remarks>
        ///// <inheritdoc cref="GetPersonByNickNameTool()" />
        //public async Task<Person?> GetPersonByNickNameWithEmail(string email, ApplicationUser user)
        //{
        //    var ufind = await _db.Users
        //        .FirstOrDefaultAsync(u => u.Email == email);

        //    if (ufind != null)
        //    {

        //    var pp = await _db.Person
        //        .Where(p => p.DexHolderId == user.DexHolder.Id)
        //        .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
        //    return pp;


        //    }

        //    return await _db.Person.Include(p => p.DexHolder)
        //            .FirstOrDefaultAsync(dh => dh.ApplicationUserName == ufind.UserName);
        //                    .ThenInclude(dh => )

        //        return await _db.DexHolder
        //            .Include(dh => dh.ApplicationUser)
        //            .Include(dh => dh.People)
        //            .ThenInclude(p => p.FullName)
        //            .Include(dh => dh.People)
        //            .ThenInclude(p => p.ContactInfo)
        //            .ThenInclude(ci => ci.SocialMedias)
        //            .Include(dh => dh.People)
        //            .ThenInclude(p => p.RecordCollector)
        //            .ThenInclude(rc => rc.EntryItems)
        //            .Include(dh => dh.People)
        //            .ThenInclude(p => p.PersonChildren)
        //            .Include(dh => dh.People)
        //            .ThenInclude(p => p.PersonParents)
        //            .FirstOrDefaultAsync(dh => dh.ApplicationUserName == ufind.UserName);



        public async Task<Person?> GetPersonByNickNameWithUser(string nickName, ApplicationUser user)
        {
            var pp = await _db.Person
                            .Include(p => p.RecordCollector)
                .Include(p => p.ContactInfo)
                .Include(p => p.FullName)
                .Include(p => p.PersonParents)
                .Include(p => p.PersonChildren)
                .Include(p => p.DexHolder)
                .Where(p => p.DexHolderId == user.DexHolder.Id)
                .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
            return pp;
        }

        public async Task<Person?> GetPersonByNickNameWithDex(string nickName, DexHolder dexHolder)
        {
            var pp = await _db.Person
                            .Include(p => p.RecordCollector)
                .Include(p => p.ContactInfo)
                .Include(p => p.FullName)
                .Include(p => p.PersonParents)
                .Include(p => p.PersonChildren)
                .Include(p => p.DexHolder)
                .Where(p => p.DexHolderId == dexHolder.Id)
                .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
            return pp;
        }




        public async Task<Person?> GetPersonByNickNameWithUserNameAsync(string userName, string nickName)
        {
            var dex = await _userRepo.GetDexHolderByUserNameAsync(userName);

            var pp = await _db.Person
                            .Include(p => p.RecordCollector)
                .Include(p => p.ContactInfo)
                .Include(p => p.FullName)
                .Include(p => p.PersonParents)
                .Include(p => p.PersonChildren)
                .Include(p => p.DexHolder)
                .Where(p => p.DexHolderId == dex.Id)
                .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
            return pp;
        }

        public async Task<Person?> GetPersonByNickNameWithEmailAsync(string email, string nickName)
        {
            var dex = await _userRepo.GetDexHolderByEmailAsync(email);

            var pp = await _db.Person
                            .Include(p => p.RecordCollector)
                .Include(p => p.ContactInfo)
                .Include(p => p.FullName)
                .Include(p => p.PersonParents)
                .Include(p => p.PersonChildren)
                .Include(p => p.DexHolder)
                .Where(p => p.DexHolderId == dex.Id)
                .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
            return pp;
        }

        public async Task<Person?> GetPersonByNickNameWithUserIdAsync(string userId, string nickName)
        {
            var dex = await _userRepo.GetDexHolderByUserIdAsync(userId);

            var pp = await _db.Person
                            .Include(p => p.RecordCollector)
                .Include(p => p.ContactInfo)
                .Include(p => p.FullName)
                .Include(p => p.PersonParents)
                .Include(p => p.PersonChildren)
                .Include(p => p.DexHolder)
                .Where(p => p.DexHolderId == dex.Id)
                .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
            return pp;
        }

        public async Task<Person?> ReadPersonByNickNameAsync(int dexHolderId, string personNickname)
        {
            var pp = await _db.Person
                            .Include(p => p.RecordCollector)
                .Include(p => p.ContactInfo)
                .Include(p => p.FullName)
                .Include(p => p.PersonParents)
                .Include(p => p.PersonChildren)
                .Include(p => p.DexHolder)
                .Where(p => p.DexHolderId == dexHolderId)
                .Where(p => p.Nickname == personNickname).FirstOrDefaultAsync();
            return pp;



        }




        public async Task<Person?> ReadPersonByIdAsync(int personId)
        {
            var person = await _db.Person
                .Include(p => p.RecordCollector)
                .Include(p => p.ContactInfo)
                .Include(p => p.FullName)
                .Include(p => p.PersonParents)
                .Include(p => p.PersonChildren)
                .Include(p => p.DexHolder)
                .FirstOrDefaultAsync(p => p.Id == personId);
            return person;
        }

        // TODO - FIX THIS METHOD TO ACCEPT AN INTEGER FOR CRITERIA
        public async Task<Person?> GetOneByUserInputAsync(string input, string inCriteria){
            Person? person = null;
            
            var testVM = await _userRepo.GetPersonPlusDexListVMAsync(input, inCriteria);
            if (testVM == null) {
                await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\tGetOneByUserInputAsync:\n\t\t GetPersonPlusDexListVMAsync({input}, {inCriteria}).\n\t\t\tChecking DexId!!!\n\n\n------------\n\n\n");
                return null;
                }

            var criteria = testVM.Nickname; // HACK - quick fix to get the nickname, just in case the inCriteria is another property.


            if (int.TryParse(input, out int idout))
            {
                await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\tGetOneByUserInputAsync:\n\t\t Input, {input}, is an int.\n\t\t\tChecking DexId!!!\n\n\n------------\n\n\n");
                person = await GetPersonByNickNameWithUserNameAsync(input, criteria);
                if (person == null) await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\t\t Criteria, {criteria}, did not match a Person's Nickname for DexHolder, {input}!\n\t\t\tReturning null...\n\n------------\n\n\n");
                else { await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\t\tPerson, {criteria}, found for user, {input}!:\n\t\t\tReturning Person, {criteria}!!!\n\n\n------------\n\n\n"); }
                await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\tGetOneByUserInputAsync:\n\t\t Input, {input}, did not match a DexHolder.\n\t\t\tBecause {input} is an int and not a string, returning null!!\n\n\n------------\n\n\n"); 
            }
            else{
                await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\tGetOneByUserInputAsync:\n\t\t Input, {input}, is NOT an int.\n\t\t\tChecking email!!!\n");
                
                // if input finds a match, try setting person
                if (await _userRepo.GetDexHolderByEmailAsync(input) != null)
                {
                    person = await GetPersonByNickNameWithEmailAsync(input,criteria);
                    if (person == null) await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\t\t Criteria, {criteria}, did not match a Person's Nickname for User, {input}!\n\t\t\tReturning null...\n\n------------\n\n\n");
                    else{ await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\t\tPerson, {criteria}, found for user, {input}!:\n\t\t\tReturning Person, {criteria}!!!\n\n\n------------\n\n\n"); }
                }
                else{ await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\tGetOneByUserInputAsync:\n\t\t Input, {input}, did not match an email.\n\t\t\tChecking username!!!\n\n\n------------\n\n\n"); }

                // if person is not already set, and if input finds a match, try setting person
                if (person == null && await _userRepo.GetDexHolderByUserNameAsync(input) != null)
                {
                    person = await GetPersonByNickNameWithUserNameAsync(input,criteria);
                    if (person == null) await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\t\t Criteria, {criteria}, did not match a Person's Nickname for User, {input}!\n\t\t\tReturning null...\n\n------------\n\n\n");
                    else{ await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\t\tPerson, {criteria}, found for user, {input}!:\n\t\t\tReturning Person, {criteria}!!!\n\n\n------------\n\n\n"); }
                }
                else{ await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\tGetOneByUserInputAsync:\n\t\t Input, {input}, did not match an UserName.\n\t\t\tChecking User Id!!!\n\n\n------------\n\n\n"); }

                // if person is not already set, and if input finds a match, try setting person
                if (person == null && await _userRepo.GetDexHolderByUserIdAsync(input) != null)
                {
                    person = await GetPersonByNickNameWithUserIdAsync(input,criteria);
                    if (person == null) await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\t\t Criteria, {criteria}, did not match a Person's Nickname for User, {input}!\n\t\t\tReturning null...\n\n------------\n\n\n");
                    else{ await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\t\tPerson, {criteria}, found for user, {input}!:\n\t\t\tReturning Person, {criteria}!!!\n\n------------\n\n\n"); }
                }
                else{ await Console.Out.WriteLineAsync($"\n\n\n------------\n\n\n\tGetOneByUserInputAsync:\n\t\t Input, {input}, did not match an userId.\n\t\t\tInput, {input}, did not match any user an any criteria.\n\t\tReturning null...\n\n\n------------\n\n\n"); }


            }
            await Console.Out.WriteLineAsync($"\n\tGetOneByUserInputAsync:\tEXITING...\n\n");
            return person;
        }


        public async Task<ContactInfo?> ReadContactInfoByIdAsync(Int64 ContactInfoId)
        {
            var contact = await _db.ContactInfo
                .Include(c => c.Person)
                .Include(c => c.SocialMedias)
                .FirstOrDefaultAsync(c => c.Id == ContactInfoId);
            return contact;
        }
        public async Task<RecordCollector?> ReadRecordByIdAsync(Int64 RecordCollectorId)
        {
            var contact = await _db.RecordCollector
                .Include(r => r.Person)
                .Include(r => r.EntryItems)
                .FirstOrDefaultAsync(r => r.Id == RecordCollectorId);
            return contact;
        }
        public async Task<ContactInfo?> ReadContactInfoByPersonIdAsync(int personId)
        {
            var contact = await _db.ContactInfo
                .Include(c => c.Person)
                .Include(c => c.SocialMedias)
                .FirstOrDefaultAsync(c => c.Person.Id == personId);
            return contact;
        }
        public async Task<RecordCollector?> ReadRecordByPersonIdAsync(int personId)
        {
            var contact = await _db.RecordCollector
                .Include(r => r.Person)
                .Include(r => r.EntryItems)
                .FirstOrDefaultAsync(r => r.Person.Id == personId);
            return contact;
        }
        // public async Task<ContactInfo?> ReadContactInfoByIdAsync(int personId)
        // {
        //     var contact = await _db.ContactInfo
        //         .Include(c => c.Person)
        //         .Include(c => c.SocialMedias)
        //         .FirstOrDefaultAsync(c => c.Person.Id == personId);
        //     return contact;
        // }
        // public async Task<RecordCollector?> ReadRecordByIdAsync(int personId)
        // {
        //     var contact = await _db.RecordCollector
        //         .Include(r => r.Person)
        //         .Include(r => r.EntryItems)
        //         .FirstOrDefaultAsync(r => r.Person.Id == personId);
        //     return contact;
        // }

    }
}
