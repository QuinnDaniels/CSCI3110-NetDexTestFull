using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;
using System.ComponentModel.DataAnnotations;

namespace NetDexTest_01.Models.ViewModels
{
    /// <summary>
    /// A view model that is used in <see cref="DbUserRepository.ReadAllApplicationUsersVMAsync" />. 
    /// </summary>
    /// <remarks>
    /// [user.username]
    /// [user.email]
    /// [user.accessFailCount]
    /// [user.DexHolder.Id]
    /// [user.DexHolder.FirstName]
    /// [user.DexHolder.LastName]
    /// </remarks>
    public class AdminUserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? DexHolderId { get; set; } = -1;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int AccessFailedCount { get; set; } = -1;
        public int PeopleCount { get; set; } = -1;
        public List<RoleVM> Roles { get; set; } = new List<RoleVM>();

    }

    public class AdminUserVMOut
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? DexHolderId { get; set; } = -1;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int AccessFailedCount { get; set; } = -1;
        public int PeopleCount { get; set; } = -1;
        public string Roles { get; set; }


        /// <summary>
        /// Provides an easy way to flatten Roles into a transferrable string that can be parsed on the front end
        /// </summary>
        /// <param name="x"></param>
        public AdminUserVMOut(AdminUserVM x) 
        {
            this.Id = x.Id;
            this.UserName = x.UserName ;
            this.Email = x.Email ;
            this.DexHolderId = x.DexHolderId ;
            this.FirstName = x.FirstName ;
            this.LastName = x.LastName ;
            this.AccessFailedCount = x.AccessFailedCount ;
            this.PeopleCount = x.PeopleCount;

            string rolesString = x.Roles.ToList().Aggregate("", (current, s) => current + (s + "+"));
            this.Roles = rolesString;
        }
    }




}
