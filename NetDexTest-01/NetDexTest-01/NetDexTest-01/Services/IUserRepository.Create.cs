using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace NetDexTest_01.Services
{
    public partial interface IUserRepository
    {

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

        /// <remarks>
        /// <para>
        /// For use with Registration API Endpoints
        /// </para>
        /// <para>
        /// To get an IdentityResult back, use <seealso cref="CreateUserDexHolderAsync(RegisterModel, Boolean)"/> and any value of Boolean will work
        /// </para>
        /// </remarks>
        /// <returns>
        /// </returns>
        /// <inheritdoc cref="CreateUserDexHolderAsync()"/>
        Task<ApplicationUser> CreateUserDexHolderAsync(
                RegisterModel registerModel);
        /// <remarks>
        /// For use with Registration API Endpoints, and returns if the creation succeeded
        /// </remarks>
        /// <returns>
        /// </returns>
        /// <inheritdoc cref="CreateUserDexHolderAsync()"/>
        Task<IdentityResult> CreateUserDexHolderAsync(
                RegisterModel registerModel, Boolean inFlag);


        #region BatchCreateComment
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
        #endregion BatchCreateComment
        Task<IEnumerable<ApplicationUser>> CreateUserDexHolderAsync(
                List<ApplicationUser> users, List<DexHolder> tempDexHolders, List<String> passwords);



        Task<AuthenticatedResponse> GetTokens(ApplicationUser user);
        string GetRefreshToken();
        Task<ApplicationUser> GetUserByRefreshToken(string refreshToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);


    }
}
