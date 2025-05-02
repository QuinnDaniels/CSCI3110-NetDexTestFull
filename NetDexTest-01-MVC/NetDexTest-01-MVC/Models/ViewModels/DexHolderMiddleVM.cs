using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Security.Cryptography.Pkcs;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NetDexTest_01_MVC.Models.Entities;


namespace NetDexTest_01_MVC.Models.ViewModels
{

    public class DexHolderUserEditVM
    {
        public string? ApplicationUserId { get; set; }
        public string ApplicationUserName { get; set; }
        public string ApplicationEmail { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Pronouns { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int DexId { get; set; }
    }

    public class DexHolderUserEditPlusVM : DexHolderUserEditVM
    {
        public int? PeopleCount { get; set; } = -1;
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


        public DexHolderUserEditVM getEditInstance()
        {
            return new DexHolderUserEditVM()
            {
                ApplicationUserId = this.ApplicationUserId,
                ApplicationUserName = this.ApplicationUserName,
                ApplicationEmail = this.ApplicationEmail,
                FirstName = this.FirstName,
                MiddleName = this.MiddleName,
                LastName = this.LastName,
                Gender = this.Gender,
                Pronouns = this.Pronouns,
                DateOfBirth = this.DateOfBirth,
                DexId = this.DexId,

            };
        }


    }


    public class DexHolderMiddleVMResponse
    {
        public HttpStatusCode Status { get; set; }
        public string? Title { get; set; } = "default title";
        public string? Message { get; set; } = "default message";
        public int? DexId { get; set; } = -1;
        public string? ApplicationUserId { get; set; }
        public string? ApplicationUserName { get; set; }
        public string? ApplicationEmail { get; set; } // from appUser
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Pronouns { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<RoleVM> Roles { get; set; } = new List<RoleVM>();
        public List<PersonDexListVM> People { get; set; } = new List<PersonDexListVM>();
        //public ApplicationUser ApplicationUser { get; set; }
        public int? PeopleCount { get; set; } = -1;

        public DexHolderMiddleVM getInstanceDexHolderMiddleVM()
        {
            DexHolderMiddleVM outDex = new()
            {
                DexId = this?.DexId ?? 0,
                ApplicationUserId = this.ApplicationUserId,
                ApplicationUserName = this.ApplicationUserName,
                ApplicationEmail = this.ApplicationEmail,
                FirstName = this.FirstName,
                MiddleName = this.MiddleName,
                LastName = this.LastName,
                Gender = this.Gender,
                Pronouns = this.Pronouns,
                DateOfBirth = this.DateOfBirth,
                Roles = this.Roles,
                People = this.People
            };
            return outDex;
        }
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
        public int? Rating { get; set; } = 0;
        public bool? Favorite { get; set; } = false;
        public int? RcEntryItemsCount { get; set; } = -1;
        public int? CiSocialMediasCount { get; set; } = -1;
        public int? PersonParentsCount { get; set; } = -1;
        public int? PersonChildrenCount { get; set; } = -1;

        public EditPersonFullVM getEditInstance()
        {
            return new EditPersonFullVM
            {
                Id = this.Id,
                Nickname = this.Nickname,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender,
                Pronouns = this.Pronouns,
                Rating = this.Rating,
                Favorite = this.Favorite,
                //Email = this.AppEmail,
                Email = null,
                NameFirst = this.NameFirst,
                NameMiddle = this.NameMiddle,
                NameLast = this.NameLast,
                PhNameFirst = this.PhNameFirst,
                PhNameMiddle = this.PhNameMiddle,
                PhNameLast = this.PhNameLast
            };

        }



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


        public DexHolderUserEditVM getEditInstance()
        {
            return new DexHolderUserEditVM()
            {
                ApplicationUserId = this.ApplicationUserId,
                ApplicationUserName = this.ApplicationUserName ,
                ApplicationEmail = this.ApplicationEmail ,
                FirstName = this.FirstName ,
                MiddleName = this.MiddleName ,
                LastName = this.LastName ,
                Gender = this.Gender ,
                Pronouns = this.Pronouns ,
                DateOfBirth = this.DateOfBirth ,
                DexId = this.DexId,

            };
        }



    }



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
        public int CiSocialMediasCount { get; set; } = -1;
        public int PersonParentsCount { get; set; } = -1;
        public int PersonChildrenCount { get; set; } = -1;

        public ICollection<SocialMediaVM>? SocialMedias { get; set; }
        public ICollection<EntryItemVM>? Entries { get; set; }


        public EditPersonFullVM getEditInstance()
        {
            return new EditPersonFullVM
            {
                Id = this.Id,
                Nickname = this.Nickname,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender,
                Pronouns = this.Pronouns,
                Rating = this.Rating,
                Favorite = this.Favorite,
                Email = this.AppEmail,
                NameFirst = this.NameFirst,
                NameMiddle = this.NameMiddle,
                NameLast = this.NameLast,
                PhNameFirst = this.PhNameFirst,
                PhNameMiddle = this.PhNameMiddle,
                PhNameLast = this.PhNameLast
            };

        }




    }




    public class PersonPlusDexListVMResponse : PersonPlusDexListVM
    {
        public HttpStatusCode Status { get; set; }
        public string? Title { get; set; } = "default title";
        public string? Message { get; set; } = "default message";



        public PersonPlusDexListVM getPlusDexInstance()
        {
            return new PersonPlusDexListVM
            {
                Id = this.Id,
                AppUsername = this.AppUsername,
                AppEmail = this.AppEmail,
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
                PersonChildrenCount = this.PersonChildrenCount,
                SocialMedias = this.SocialMedias,
                Entries = this.Entries
            };
        }










    }


    /*--------------------------*/



    public class SocialMediaVMResponse : SocialMediaVM
    {
        public HttpStatusCode Status { get; set; }
        public string? Title { get; set; } = "default title";
        public string? Message { get; set; } = "default message";

    }
    public class SocialMediaVM
    {

        public Int64 Id { get; set; }
        [StringLength(120)]
        public string CategoryField { get; set; } = string.Empty;
        public string? SocialHandle { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime LogTimestamp { get; set; }// = DateTime.Now;

    }


    /*-----------------------------*/

    public class EntryItemVMResponse : EntryItemVM
    {
        public HttpStatusCode Status { get; set; }
        public string? Title { get; set; } = "default title";
        public string? Message { get; set; } = "default message";

    }
    public class EntryItemVM
    {

        public Int64 Id { get; set; }
        /// <summary>
        /// A short title to easily identify an Entry. Nullable, because on
        /// the frontend, if null, it should be replaced with a breif preview of flavortext 
        /// </summary>
        [StringLength(240)]
        public string? ShortTitle { get; set; }
        [StringLength(456)]
        public string FlavorText { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        public DateTime LogTimestamp { get; set; }// = DateTime.Now;
    }





    //public class Rootobject
    //{
    //    public int Id { get; set; }
    //    public string AppUsername { get; set; }
    //    public string AppEmail { get; set; }
    //    public int LocalCounter { get; set; }
    //    public int DexId { get; set; }
    //    public string Nickname { get; set; }
    //    public string NameFirst { get; set; }
    //    public string NameMiddle { get; set; }
    //    public string NameLast { get; set; }
    //    public string PhNameFirst { get; set; }
    //    public string PhNameMiddle { get; set; }
    //    public string PhNameLast { get; set; }
    //    public object DateOfBirth { get; set; }
    //    public string Gender { get; set; }
    //    public string Pronouns { get; set; }
    //    public int Rating { get; set; }
    //    public bool Favorite { get; set; }
    //    public int RcEntryItemsCount { get; set; }
    //    public int CiSocialMediasCount { get; set; }
    //    public int PersonParentsCount { get; set; }
    //    public int PersonChildrenCount { get; set; }
    //    public Socialmedia[] SocialMedias { get; set; }
    //    public Entry[] Entries { get; set; }
    //}










}

