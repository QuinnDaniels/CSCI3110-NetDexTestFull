using NetDexTest_01.Models.Entities;

namespace NetDexTest_01.Services
{
    // TODO: do this!
    /// <summary>
    /// interacts with Extensions of Person (Ci, Fn, Rc)
    /// </summary>
    public partial interface IPersonRepository
    {
        
        void GetPersonByNickName();
        /// <summary>
        /// <para>
        /// Uses the Index on Person to find the appropriate record. Has many overloads.
        /// </para>
        /// <para>
        /// Using a combination of data from ApplicationUser and/or DexHolder with Person.Nickname to find the unique ( Nickname, DexHolderId ) combination
        /// </para>
        /// </summary>
        /// <remarks> NOTICE: Empty parameters is only for documentation purposes!!! </remarks>
        /// <exception cref="NotImplementedException"></exception>
        void GetPersonByNickNameTool();
        /// <remarks>
        /// Uses an ApplicationUser object to access ApplicationUser.DexHolder directly to search for a person with the DexHolderId
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickName()" />

        Task<Person?> GetPersonByNickName(string nickName, ApplicationUser user);
        /// <remarks>
        /// Uses a DexHolder object to access DexHolderId directly to search for a person with the DexHolderId
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickName()" />
        Task<Person?> GetPersonByNickName(string nickName, DexHolder dex);
        Task<Person?> GetPersonByNickName(PropertyField pType, string inputProperty, string nickName);
        Task<Person?> GetPersonByNickNameWithUser(string nickName, ApplicationUser user);
        Task<Person?> GetPersonByNickNameWithDex(string nickName, DexHolder dexHolder);
        Task<Person?> GetPersonByNickNameWithUserNameAsync(string userName, string nickName);
        Task<Person?> GetPersonByNickNameWithUserIdAsync(string userId, string nickName);
        Task<Person?> ReadPersonByNickNameAsync(int dexHolderId, string personNickname);
        Task<Person?> ReadPersonByIdAsync(int personId);


    }
}
