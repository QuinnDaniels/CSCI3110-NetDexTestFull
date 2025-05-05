using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;
using System.ComponentModel.DataAnnotations;


namespace NetDexTest_01.Models.ViewModels
{
    public class DexHolderUserEditVM
    {
        public int DexId { get; set; }
        public string? ApplicationUserId { get; set; }
        public string? ApplicationUserName { get; set; }
        public string? ApplicationEmail { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Pronouns { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    public class DexHolderUserVM : DexHolderUserEditVM
    {
        public string? ApplicationUserId { get; set; }
        public int PeopleCount { get; set; } = -1;

    }


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
        public int? LocalCounter { get; set; }
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


    /*---------------------------------------------*/



    public class DexHolderPlusVM
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
        public List<PersonPlusDexListVM> People { get; set; } = new List<PersonPlusDexListVM>();
        //public ApplicationUser ApplicationUser { get; set; }
        public int PeopleCount { get; set; } = -1;
    }


    /* TODO add ContactInfo notes to this view. 
    Upon inspection I realized that I had set it up in an odd way, so it makes more
    sense to add the column to the person rather than the individual social items. 
    */

    public class PersonPlusDexListVM
    {
        public int Id { get; set; }
        public string AppUsername { get; set; }
        public string AppEmail { get; set; }
        public int? LocalCounter { get; set; }
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
        public Int64 RecordCollectorId { get; set; } = -1;
        public int CiSocialMediasCount { get; set; } = -1;
        public Int64 ContactInfoId { get; set; } = -1;
        public int PersonParentsCount { get; set; } = -1;
        public int PersonChildrenCount { get; set; } = -1;

        // NOTE How do I create a way to add ContactInfo notes
        public List<SocialMediaVM> SocialMedias { get; set; } = new();
        //public List<SocialMediaVMNotes> SocialMedias { get; set; } = new();
        public List<EntryItemVM> Entries { get; set; } = new();
        public PersonDexListVM getPersonDexVMInstance()
        {
            return new PersonDexListVM
            {
                Id = this.Id,
                //AppUsername = this.AppUsername,
                //AppEmail = this.AppEmail,
                LocalCounter = this.LocalCounter,
                DexId = this.DexId,
                Nickname = this.Nickname,
                NameFirst = this.NameFirst,
                NameMiddle = this.NameMiddle,
                NameLast = this.NameLast,
                PhNameFirst = this.PhNameFirst,
                PhNameMiddle = this.PhNameMiddle,
                PhNameLast = this.PhNameLast,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender,
                Pronouns = this.Pronouns,
                Rating = this.Rating,
                Favorite = this.Favorite,
                RcEntryItemsCount = this.RcEntryItemsCount,
                CiSocialMediasCount = this.CiSocialMediasCount,
                PersonParentsCount = this.PersonParentsCount,
                PersonChildrenCount = this.PersonChildrenCount
            };

        }



    }






}
