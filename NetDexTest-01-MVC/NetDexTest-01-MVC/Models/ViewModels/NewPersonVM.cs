using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Security.Cryptography.Pkcs;
using NetDexTest_01_MVC.Models.Entities;

namespace NetDexTest_01_MVC.Models.ViewModels
{
    public class NewPersonVM
    {

        /// <summary>
        /// a factory method.<br />
        /// 
        /// - gets discussed in software engineering, design patterns.<br />
        /// 
        /// - all factory methods do is create things
        /// </summary>
        /// <returns></returns>
        public Person GetPersonInstance()
        {
            return new Person
            {
                Id = 0,
                DexHolderId = 0,
                Nickname = this.Nickname,   //this.Nickname is passed from constructor;  Nickname is what will be the property of the new person
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender,
                Pronouns = this.Pronouns,
                Rating = this.Rating,
                Favorite = this.Favorite
            };
        }



        [StringLength(256)]
        public string Nickname { get; set; } = string.Empty;



        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; } = DateTime.Now;

        [StringLength(120)]
        public string? Gender { get; set; } = String.Empty;
        [StringLength(25)]
        public string? Pronouns { get; set; } = String.Empty;

        [Range(0.0, 10.0, ErrorMessage = "Must be a value from 0 to 10")]
        public int Rating { get; set; } = 0; // Eventually convert into an enum

        public bool Favorite { get; set; } = false;


        public string? Email { get; set; } = String.Empty;


    }


/*-----------------------------------*/


    public class PersonDetailsVM
    {
        public int Id { get; set; }
        public string? AppUsername { get; set; }
        public string? AppEmail { get; set; }
        public int? LocalCounter { get; set; }
        public int? DexId { get; set; }
        public string? Nickname { get; set; }
        public string? NameFirst { get; set; }
        public string? NameMiddle { get; set; }
        public string? NameLast { get; set; }
        public string? PhNameFirst { get; set; }
        public string? PhNameMiddle { get; set; }
        public string? PhNameLast { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Pronouns { get; set; }
        public int? Rating { get; set; }
        public bool? Favorite { get; set; }
        public int? RcEntryItemsCount { get; set; }
        public int? CiSocialMediasCount { get; set; }
        public int? PersonParentsCount { get; set; }
        public int? PersonChildrenCount { get; set; }
        //public Person GetPersonInstance()
        //{
        //    return new Person
        //    {
        //        Id = 0,
        //        DexHolderId = 0,
        //        Nickname = this.Nickname,   //this.Nickname is passed from constructor;  Nickname is what will be the property of the new person
        //        DateOfBirth = this.DateOfBirth,
        //        Gender = this.Gender,
        //        Pronouns = this.Pronouns,
        //        Rating = this.Rating,
        //        Favorite = this.Favorite
        //    };
        //}

        public FullName GetNameInstance()
        {
            return new FullName
            {
                NameFirst = this.NameFirst,
                NameMiddle = this.NameMiddle,
                NameLast = this.NameLast,
                PhNameFirst = this.PhNameFirst,
                PhNameMiddle = this.PhNameMiddle,
                PhNameLast = this.PhNameLast,
                PersonId = this.Id
            };
        }

    }


    public class PersonDetailsRequest : PersonDetailsVM
    {
        public HttpStatusCode Status { get; set; }
        public string? Title { get; set; } = "default title";
        public string? Message { get; set; } = "default message";
    }





    // HACK this should probably also be containing password but whatever...
    public class PersonRequest
    {
        public string UserInput { get; set; } = string.Empty;
        public string Criteria { get; set; } = string.Empty;
    }




    public class EditPersonVM
    {
        public int Id { get; set; }
        [StringLength(256)]
        public string? Nickname { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(120)]
        public string? Gender { get; set; }
        [StringLength(25)]
        public string? Pronouns { get; set; }

        [Range(0.0, 10.0, ErrorMessage = "Must be a value from 0 to 10")]
        public int? Rating { get; set; } // Eventually convert into an enum

        public bool? Favorite { get; set; }
        // TODO: Change this to a generic input?
        public string? Email { get; set; }

    }

    public class EditPersonFullVM : EditPersonVM
    {
        [StringLength(120)]
        public string? NameFirst { get; set; }
        [StringLength(120)]
        public string? NameMiddle { get; set; }
        [StringLength(120)]
        public string? NameLast { get; set; }
        [StringLength(120)]
        public string? PhNameFirst { get; set; }
        [StringLength(120)]
        public string? PhNameMiddle { get; set; }
        [StringLength(120)]
        public string? PhNameLast { get; set; }

    }









    public class RelationshipRequest
    {
        public string input { get; set; }
        public string nicknameOne { get; set; }
        public string nicknameTwo { get; set; }
        public string? description { get; set; }
    }


    public class RelationshipVM
    {
        public RelationshipVM()
        {

        }

        public string? AppEmail { get; set; }
        public int? Id { get; set; } = 0;
        public string AppUsername { get; set; }
        public int? PersonParentId { get; set; }
        public string? ParentNickname { get; set; }
        public string RelationshipDescription { get; set; } = string.Empty;
        public int? PersonChildId { get; set; }
        public string? ChildNickname { get; set; }
        public DateTime? LastUpdated { get; set; }


    }





}
