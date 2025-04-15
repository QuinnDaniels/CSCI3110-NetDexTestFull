using NetDexTest_01.Models.Entities;

namespace NetDexTest_01.Services
{
    public partial interface IUserRepository
    {

        // READ - ONE
        // ----------------------

        /// <summary>
        /// READ ONE DexUser using a field from ApplicationUser. Should allow somewhat easier use when paired with UserManager
        /// </summary>
        /// <remarks>
        /// <para>
        /// Polymorphic. Decided to combine functionality of other methods with a single one
        /// </para>
        /// <para>PropertyField choices: [ id | username ] </para>
        /// </remarks>
        /// <param name="pType">[ id | username ]</param>
        /// <param name="input">username or id to find with</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        Task<DexHolder?> ReadDexAsync(PropertyField pType, string input);
        /// <remarks> Search only using user Id </remarks>
        /// <param name="applicationUserId">dbo.User.Id</param>
        /// <returns>DexHolder</returns>
        /// <inheritdoc cref="ReadDexAsync(PropertyField, string)"/>
        Task<DexHolder?> ReadDexByIdAsync(string id);
        /// <remarks> Search only using username </remarks>
        /// <param name="username"></param>
        /// <returns>DexHolder</returns>
        /// <inheritdoc cref="ReadDexAsync(PropertyField, string)"/>
        Task<DexHolder?> ReadDexByUsernameAsync(string username);


    }
}
