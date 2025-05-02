using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Services;
using NetDexTest_01.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetDexTest_01.Models.ViewModels
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
        public string GetAttachedEmail()
        {
            return Email;
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

        // TODO: Change this to a generic input?
        public string? Email { get; set; } = String.Empty;




    }

    // HACK this should probably also be containing password but whatever...
    public class PersonRequest
    {
        public string UserInput { get; set; } = string.Empty;
        public string Criteria { get; set; } = string.Empty;
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


    public class NewPersonRequestVM
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
        public NewPersonVM GetNewPersonVMInstance()
        {
            return new NewPersonVM
            {
                Nickname = this.Nickname,   //this.Nickname is passed from constructor;  Nickname is what will be the property of the new person
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender,
                Pronouns = this.Pronouns,
                Rating = this.Rating,
                Favorite = this.Favorite,
                Email = this.Email
            };
        }
        public string GetAttachedEmail()
        {
            return Email;
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

        // TODO: Change this to a generic input?
        public string? Email { get; set; } = String.Empty;

        public string? __RequestVerificationToken = string.Empty;


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



}
