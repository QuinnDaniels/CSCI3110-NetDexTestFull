using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Security.Cryptography.Pkcs;
using NetDexTest_01_MVC.Models.Entities;


namespace NetDexTest_01_MVC.Models.ViewModels
{

    public class DexHolderMiddleVM
    {
        public int DexId { get; set; } = -1;
        public string ApplicationUserId { get; set; }
        public string ApplicationUserName { get; set; }
        public string ApplicationEmail { get; set; } // from appUser
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Pronouns { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<RoleVM> Roles { get; set; } = new List<RoleVM>();
        public List<PersonDexListVM> People { get; set; } = new List<PersonDexListVM>();
        //public ApplicationUser ApplicationUser { get; set; }
        public int PeopleCount { get; set; } = -1;
    }


    public class PersonDexListVM
    {
        public int Id { get; set; }
        public int DexId { get; set; }
        public string Nickname { get; set; }
        public string? NameFirst { get; set; }
        public string? NameMiddle { get; set; }
        public string? NameLast { get; set; }
        public string? PhNameFirst { get; set; }
        public string? PhNameMiddle { get; set; }
        public string? PhNameLast { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Pronouns { get; set; }
        public int Rating { get; set; } = 0;
        public bool Favorite { get; set; } = false;
        public int RcEntryItemsCount { get; set; } = -1;
        public int CiSocialMediasCount { get; set; } = -1;
        public int PersonParentsCount { get; set; } = -1;
        public int PersonChildrenCount { get; set; } = -1;

    }

}

