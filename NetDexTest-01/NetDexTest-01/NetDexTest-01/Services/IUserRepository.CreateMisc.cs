using NetDexTest_01.Models.Entities;

namespace NetDexTest_01.Services
{
    public partial interface IUserRepository
    {

        #region CreateTools

        // CREATE (Misc + Overrides)
        // --------------------------

        /// <summary>
        /// Create a User and DexHolder in the database at the same time, including password. Offers various overloads
        /// 
        ///  
        /// </summary>
        /// <remarks>
        /// <para>
        /// <code>
        /// WARNING! Must include parameters! Empty Arguments is only for documentation purposes!
        /// </code>
        /// </para>
        /// <br/><br/>
        /// For the recommended usage see the documentation of the 
        /// <see cref="CreateUserDexHolderAsync(ApplicationUser, DexHolder, string)">
        /// current working overload
        /// </see>
        /// on how to use with ApplicationUser and DexHolder objects.
        /// <br/><br/>
        /// Also allows for 
        /// <see cref="CreateUserDexHolderAsync(List{ApplicationUser}, List{DexHolder}, List{string})">
        /// Batch Creation
        /// </see>
        /// using List&lt;&gt;. Useful for Seeding the database.
        /// </remarks>
        /// <returns>NotImplementedException()</returns>
        /// <param name="password">Password to add user into the UserManager</param>
        /// <param name="email">unique string email address</param>
        /// <param name="username">unique string Username</param>
        /// <param name="userAndEmail">When the username and email will be the same</param>
        /// <param name="user">ApplicationUser object</param>
        /// <param name="tempDexHolder">Temporary DexHolder object created before method call</param>
        /// <param name="users">List of new Users to Batch add</param>
        /// <param name="tempDexHolders">List of new DexHolders for each User</param>
        /// <param name="passwords">List of Passwords for each User + DexHolder</param>
        /// <exception cref="NotImplementedException"> Use overloads! </exception>
        Task<ApplicationUser> CreateUserDexHolderAsync();

        /// <remarks>
        /// <para>
        /// <code>DEPRECATED! Reccommended to use <see cref="CreateUserDexHolderAsync(ApplicationUser, DexHolder, string)">
        /// Current Working overload</see> instead</code>
        /// (adapted from Login.cshtml.cs.)
        /// To create an ApplicationUser and DexHolder at the same time. Creates a DexHolder with only null values
        /// </para> 
        /// </remarks>
        /// <returns></returns>
        /// <inheritdoc cref="CreateUserDexHolderAsync()"/>
        Task<ApplicationUser> CreateUserDexHolderAsync(ApplicationUser user, string password);
        Task<ApplicationUser> CreateUserDexHolderAsync(string userAndEmail, string password);
        Task<ApplicationUser> CreateUserDexHolderAsync(string username, string email, string password);

        /// <summary>
        /// <h2>DEPRECATED</h2>
        /// <para>Helper method to create a temporary DexHolder Object</para>
        /// </summary>
        /// <remarks> Create a Temporary DexHolder object. Must set ApplicationUserId and ApplicationUserName Externally!</remarks>
        /// <returns>temporary DexHolder without UserName or UserId</returns>
        DexHolder CreateTempDexHolder(string? firstName = null, string? middleName = null, string? lastName = null, string? gender = null, string? pronouns = null);
        Task<DexHolder> CreateDexAsync(DexHolder dexHolder);

        #endregion CreateTools

    }
}
