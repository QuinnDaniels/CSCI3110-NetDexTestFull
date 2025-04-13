using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetDexTest_01.Models.Entities
{
    /// <summary>
    /// The application user. Inherits and replaces IdentityUser
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        //public string FirstName { get; set; } = String.Empty;
        //public string LastName { get; set; } = String.Empty;


        [NotMapped]
        public ICollection<string> Roles { get; set; }
            = new List<string>();

        public bool HasRole(string roleName)
        {
            return Roles.Any(r => r == roleName);
        }

        public string RefreshToken { get; internal set; }

        // Navigation property for DexHolder
        public virtual DexHolder? DexHolder { get; set; }



    }
}
