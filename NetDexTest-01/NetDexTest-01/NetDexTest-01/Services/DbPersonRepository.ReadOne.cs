using Microsoft.EntityFrameworkCore;
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
        /// <remarks>
        /// Uses a
        /// <see cref="PropertyField"/>
        /// [ <see cref="PropertyField.id"/> | <see cref="PropertyField.username"/> ]
        /// to search for a <see cref="DexHolder"/> with which it combines
        /// with the nickName to find a <see cref="Person"/>
        /// 
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickName()" />
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
                //case PropertyField.email:
                //    break;
                default:
                    throw new ArgumentException();
                    break;
            }
            return person;
        }


        /// <remarks> Used by: <code><seealso cref="GetPersonByNickName(string, ApplicationUser)"/></code> </remarks>
        /// <inheritdoc cref="GetPersonByNickNameTool()" />
        public async Task<Person?> GetPersonByNickNameWithUser(string nickName, ApplicationUser user)
        {
            var pp = await _db.Person
                .Where(p => p.DexHolderId == user.DexHolder.Id)
                .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
            return pp;
        }

        /// <remarks> Used by: <code><seealso cref="GetPersonByNickName(string, DexHolder)"/></code> </remarks>
        /// <inheritdoc cref="GetPersonByNickNameTool()" />
        public async Task<Person?> GetPersonByNickNameWithDex(string nickName, DexHolder dexHolder)
        {
            var pp = await _db.Person
                .Where(p => p.DexHolderId == dexHolder.Id)
                .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
            return pp;
        }

        /// <remarks>
        /// Used by: <code><seealso cref="GetPersonByNickName(PropertyField, string, string)"/></code>
        /// with <see cref="PropertyField.username" />
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickNameTool()" />
        public async Task<Person?> GetPersonByNickNameWithUserNameAsync(string userName, string nickName)
        {
            var dex = await _userRepo.GetDexHolderByUserNameAsync(userName);

            var pp = await _db.Person
                .Where(p => p.DexHolderId == dex.Id)
                .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
            return pp;
        }

        /// <remarks>
        /// Used by: <code><seealso cref="GetPersonByNickName(PropertyField, string, string)"/></code>
        /// with <see cref="PropertyField.id" />
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickNameTool()" />
        public async Task<Person?> GetPersonByNickNameWithUserIdAsync(string userId, string nickName)
        {
            var dex = await _userRepo.GetDexHolderByUserIdAsync(userId);

            var pp = await _db.Person
                .Where(p => p.DexHolderId == dex.Id)
                .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
            return pp;
        }

        /// <summary>
        /// Uses the Index to find a Person. Looks for the record with a unique (NickName, DexId) combination
        /// </summary>
        /// <param name="dexHolderId"></param>
        /// <param name="personNickname"></param>
        /// <returns></returns>
        public async Task<Person?> ReadPersonByNickNameAsync(int dexHolderId, string personNickname)
        {
            var pp = await _db.Person
                .Where(p => p.DexHolderId == dexHolderId)
                .Where(p => p.Nickname == personNickname).FirstOrDefaultAsync();
            return pp;

        }

        /// <summary>
        /// Find a Person using the primary key of Persons
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<Person?> ReadPersonByIdAsync(int personId)
        {
            var person = await _db.Person.FirstOrDefaultAsync(p => p.Id == personId);
            return person;
        }
    }
}
