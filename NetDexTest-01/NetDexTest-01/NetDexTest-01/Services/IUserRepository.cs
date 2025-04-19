using NetDexTest_01.Models.Entities;

namespace NetDexTest_01.Services
{
    /// <summary>
    /// Interface that defines the user repository.
    /// </summary>
    /// <remarks>
    /// 
    /// <code>IUserRepository.cs</code> has child files:
    /// 
    /// <list type="bullet">
    /// <listheader>
    /// <!--Contains children(partial) files:-->
    /// </listheader>
    /// <!---->
    /// <item>
    /// <term> IUserRepository.GPT.cs </term>
    /// <description>  </description>
    /// </item>
    /// <!---->
    /// <item>
    /// <term> IUserRepository.DexReadOne.cs </term>
    /// <description> ReadOne for DexHolder </description>
    /// </item>
    /// <!---->
    /// <item>
    /// <term> IUserRepository.UserReadOne.cs </term>
    /// <description> ReadOne for ApplicationUser </description>
    /// </item>
    /// <!---->
    /// <item>
    /// <term> IUserRepository.CreateMisc.cs </term>
    /// <description> Create - Misc tool methods and overloads. </description>
    /// </item>
    /// <!---->
    /// <item>
    /// <term> IUserRepository.Create.cs </term>
    /// <description> Create - Primary working Create and Batch Creation methods. </description>
    /// </item></list>
    /// <!---->
    /// </remarks>

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



        /// <summary>
        /// get user. > eager load DexHolder > eager load People
        /// </summary>
        /// <returns></returns>
        Task<ICollection<ApplicationUser>> ReadAllUserDexPeopleAsync();

        /// <summary>
        /// get dexholder > eager load people
        /// </summary>
        /// <returns></returns>
        Task<ICollection<DexHolder>> ReadAllDexHoldersAsync();




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
