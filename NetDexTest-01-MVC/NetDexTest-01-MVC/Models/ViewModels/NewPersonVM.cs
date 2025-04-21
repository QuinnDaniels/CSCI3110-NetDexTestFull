using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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





    }
}
