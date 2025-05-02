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
        /// <remarks>
        /// Uses a
        /// <see cref="PropertyField"/>
        /// [ <see cref="PropertyField.id"/> | <see cref="PropertyField.username"/> ]
        /// to search for a <see cref="DexHolder"/> with which it combines
        /// with the nickName to find a <see cref="Person"/>
        /// 
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickName()" />
        Task<Person?> GetPersonByNickName(PropertyField pType, string inputProperty, string nickName);
        
        /// <remarks> Used by: <code><seealso cref="GetPersonByNickName(string, ApplicationUser)"/></code> </remarks>
        /// <inheritdoc cref="GetPersonByNickNameTool()" />
        Task<Person?> GetPersonByNickNameWithUser(string nickName, ApplicationUser user);

        /// <remarks> Used by: <code><seealso cref="GetPersonByNickName(string, DexHolder)"/></code> </remarks>
        /// <inheritdoc cref="GetPersonByNickNameTool()" />
        Task<Person?> GetPersonByNickNameWithDex(string nickName, DexHolder dexHolder);

        /// <remarks>
        /// Used by: <code><seealso cref="GetPersonByNickName(PropertyField, string, string)"/></code>
        /// with <see cref="PropertyField.username" />
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickNameTool()" />
        Task<Person?> GetPersonByNickNameWithUserNameAsync(string userName, string nickName);

        /// <remarks>
        /// Used by: <code><seealso cref="GetPersonByNickName(PropertyField, string, string)"/></code>
        /// with <see cref="PropertyField.email" />
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickNameTool()" />
        Task<Person?> GetPersonByNickNameWithEmailAsync(string email, string nickName);

        /// <remarks>
        /// Used by: <code><seealso cref="GetPersonByNickName(PropertyField, string, string)"/></code>
        /// with <see cref="PropertyField.id" />
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickNameTool()" />
        Task<Person?> GetPersonByNickNameWithUserIdAsync(string userId, string nickName);

        /// <summary>
        /// Uses the Index to find a Person. Looks for the record with a unique (NickName, DexId) combination
        /// </summary>
        /// <param name="dexHolderId"></param>
        /// <param name="personNickname"></param>
        /// <returns></returns>
        Task<Person?> ReadPersonByNickNameAsync(int dexHolderId, string personNickname);

        /// <summary>
        /// Find a Person using the primary key of Persons
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<Person?> ReadPersonByIdAsync(int personId);


    }
}
