using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;

namespace NetDexTest_01.Services
{
    // assisted partially with chatGPT
    public partial class DbUserRepository : IUserRepository
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="input"></param>
        /// <param name="dexHolder"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task UpdateDexAsync(PropertyField pType, string input, DexHolder dexHolder)
        {
            switch (pType)
            {
                case PropertyField.id:
                    await UpdateDexByIdAsync(input, dexHolder);
                    break;
                case PropertyField.username:
                    await UpdateDexByUsernameAsync(input, dexHolder);
                    break;
                default:
                    throw new InvalidOperationException("PropertyField is invalid! Check the switch-case and try again");
            }
        }


        public async Task UpdateDexByIdAsync(string id, DexHolder updatedDexHolder)
        {
            var dexHolderToUpdate = await ReadDexByIdAsync(id);
            dexHolderToUpdate!.FirstName = updatedDexHolder.FirstName; //Exclamation marks means we are guarenteeing it isn't null.
            dexHolderToUpdate!.LastName = updatedDexHolder.LastName;
            dexHolderToUpdate!.MiddleName = updatedDexHolder.MiddleName;
            dexHolderToUpdate!.DateOfBirth = updatedDexHolder.DateOfBirth;
            dexHolderToUpdate!.Gender = updatedDexHolder.Gender;
            dexHolderToUpdate!.Pronouns = updatedDexHolder.Pronouns;

            _db.SaveChanges();
        }

        public async Task UpdateDexByUsernameAsync(string username, DexHolder updatedDexHolder)
        {

            var dexHolderToUpdate = await ReadDexByUsernameAsync(username);
            dexHolderToUpdate!.FirstName = updatedDexHolder.FirstName; //Exclamation marks means we are guarenteeing it isn't null.
            dexHolderToUpdate!.LastName = updatedDexHolder.LastName;
            dexHolderToUpdate!.MiddleName = updatedDexHolder.MiddleName;
            dexHolderToUpdate!.DateOfBirth = updatedDexHolder.DateOfBirth;
            dexHolderToUpdate!.Gender = updatedDexHolder.Gender;
            dexHolderToUpdate!.Pronouns = updatedDexHolder.Pronouns;

            _db.SaveChanges();
        }


        public async Task UpdateUserWithDexAsync(string username, DexHolder dexHolder)
        {
            var userToUpdate = await ReadByUsernameAsync(username);
            userToUpdate!.DexHolder = dexHolder;
            //_userManager.ChangePasswordAsync;
            //_userManager.SetEmailAsync;
            //_userManager
            _db.SaveChanges();
        }


        /* TODO: Add UPDATE User information
         * 
         * Functions:
         *      - Update username, email, and/or password
         *          (password needs to be changed using UserManager)
         *      - Update corresponding DexHolder to maintain FK functionality
        */


        #region UpdateUser
        //public async verifyPassword

        //public async Task UpdateUserByUsername(PropertyField pType, string oldInput, ApplicationUser updatedUser)
        //{ }


        //public async Task UpdateUserByUsername(string oldUsername, string? oldPassword = null, string? newUsername = null, string? newPassword = null, ApplicationUser updatedUser)
        //{
        //    var userToUpdate = ReadUserAsync(PropertyField.username, oldUsername);
        //    var dexHolderToUpdate = ReadDexAsync(PropertyField.username, oldUsername);


        //    _userManager.CheckPasswordAsync

        //}
        #endregion UpdateUser


    }
}
