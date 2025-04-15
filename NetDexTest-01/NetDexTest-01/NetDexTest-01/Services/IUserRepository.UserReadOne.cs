using NetDexTest_01.Models.Entities;

namespace NetDexTest_01.Services
{
    public partial interface IUserRepository
    {
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


    }
}
