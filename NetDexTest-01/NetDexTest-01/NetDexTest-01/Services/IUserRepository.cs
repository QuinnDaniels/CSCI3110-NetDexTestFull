using NetDexTest_01.Models.Entities;

namespace NetDexTest_01.Services
{
    /// <summary>
    /// Interface that defines the user repository.
    /// </summary>
    public partial interface IUserRepository
    {

        /// <summary>
        /// Assigns the user to role async.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="roleName">The role name.</param>
        /// <returns>A Task.</returns>
        Task AssignUserToRoleAsync(string userName, string roleName);

        // READ ALL
        // -------------------
        Task<ICollection<ApplicationUser>> ReadAllApplicationUsers();
        Task<ICollection<DexHolder>> ReadAllDexHolders();
        Task<ICollection<DexHolder>> ReadAllDexAsync();


        // CREATE
        // -------------------
        /// <summary>
        /// Creates the user async.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>A Task.</returns>
        Task<ApplicationUser> CreateAsync(ApplicationUser user, string password);
        /// <summary>
        /// <para>
        /// To create an ApplicationUser and DexHolder at the same time.
        /// </para>
        /// README! Use the following code (see: remarks) before using this method:
        /// </summary>
        /// <remarks>
        /// <code>var tempDexHolder = new DexHolder{ firstName="", middleName="", lastName="", gender="", pronouns="" };</code>
        /// or
        /// <code>var tempDexHolder = new DexHolder();</code>
        /// or
        /// <code>var tempDexHolder = _userRepo.CreateTempDexHolder( firstName: "", middleName: "", lastName: "", gender: "", pronouns: "");</code>
        /// </remarks>
        /// <returns></returns>
        /// <inheritdoc cref="CreateUserDexHolderAsync()"/>
        Task<ApplicationUser> CreateUserDexHolderAsync(
                ApplicationUser user, DexHolder tempDexHolder, string password);
        // -----------------
        // BATCH CREATE
        // ------------------
        /// <remarks>
        /// 
        /// <para> Batch create a list of Users
        /// <code>
        /// var users = new List&lt;ApplicationUser&gt; {
        /// </code>
        /// <code>{</code>
        /// <code>new ApplicationUser {
        /// Email = &quot;email1@email.com&quot;,
        /// UserName = &quot;username1&quot;  },</code>
        /// <code>new ApplicationUser {
        /// Email = &quot;email2@email2.com&quot;,
        /// UserName = &quot;username2&quot;  }</code>
        /// <code>};</code>
        /// <para>and Create a list of temporary DexHolders</para>
        /// <code>
        /// var dexHolders = new List&lt;DexHolder&gt;
        /// </code>
        /// <code>{</code>
        /// <code>new DexHolder {
        /// FirstName = &quot;Jane&quot;,
        /// LastName = &quot;Doe&quot;,
        /// Gender = &quot;Woman&quot;  },</code>
        /// <code>new DexHolder {
        /// FirstName = &quot;John&quot;,
        /// LastName = &quot;Doe&quot;,
        /// Gender = &quot;Man&quot;  }</code>
        /// <code>};</code>
        /// <para>and Create a list of Password strings</para>
        /// <code>
        /// List&lt;String&gt; passwords = new List&lt;string&gt; { &quot;string&quot;, &quot;string&quot; };
        /// </code>
        /// </para>
        /// <para>README: It is recommended that the lists are of the same length, as they will be using Zip to iterate through as Tuples. It will only be evaluated to the length of the shortest array.</para>
        /// </remarks>
        /// <returns>Returns a list of created ApplicationUsers</returns>
        /// <inheritdoc cref="CreateUserDexHolderAsync(ApplicationUser, DexHolder, string)"/>
        Task<IEnumerable<ApplicationUser>> CreateUserDexHolderAsync(
                List<ApplicationUser> users, List<DexHolder> tempDexHolders, List<String> passwords);



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



        #region ReadOne
        // READ - ONE
        // ----------------------
        Task<ApplicationUser?> GetUserAsync(PropertyField pType, string input);
        /// <summary>
        /// READS the user by username async. Accesses dbo.AspNetUsers directly.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>A Task.</returns>
        Task<ApplicationUser?> GetByUsernameAsync(string username);
        Task<ApplicationUser?> GetByIdAsync(string id);

        Task<ApplicationUser?> ReadUserAsync(PropertyField pType, string input);
        /// <summary>
        /// READS the user by username async. Accesses UserManager
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>A Task.</returns>
        Task<ApplicationUser?> ReadByUsernameAsync(string username);
        Task<ApplicationUser?> ReadByIdAsync(string id);


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

        #endregion ReadOne


        //Task<ApplicationUser?> GetUserByEmailAsync(string email);





        //Task<DexHolder?> ReadDexAsync(int id);


        /// <summary>
        /// Update a 
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="input"></param>
        /// <param name="dexHolder"></param>
        /// <returns></returns>
        Task UpdateDexAsync(PropertyField pType, string input, DexHolder dexHolder);
        Task UpdateDexByIdAsync(string id, DexHolder dexHolder);
        Task UpdateDexByUsernameAsync(string username, DexHolder dexHolder);

        Task UpdateUserWithDexAsync(string username, DexHolder dexHolder);



        Task DeleteAsync(PropertyField pType, string input);
        Task DeleteByIdAsync(string id);
        Task DeleteByUsernameAsync(string username);



    }
}
