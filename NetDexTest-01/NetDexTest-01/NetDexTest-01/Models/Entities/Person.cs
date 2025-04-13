using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NetDexTest_01.Models.Entities
{
    [Index(nameof(Nickname), IsUnique = true)]
    public class Person
    {
        public int Id { get; set; }

        [Required]
        
        [StringLength(256)]
        public string Nickname { get; set; }



        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; } = DateTime.Now;

        [StringLength(120)]
        public string? Gender { get; set; } = String.Empty;
        [StringLength(25)]
        public string? Pronouns { get; set; } = String.Empty;

        public int Ratings { get; set; } = 0; // Eventually convert into an enum

        public bool Favorite { get; set; } = false;
        /// <summary>
        /// FK to DexHolderId
        /// </summary>
        public int DexHolderId { get; set; } 
        /// <summary>
        /// Navigation property from Person to DexHolder
        /// </summary>
        public DexHolder DexHolder { get; set; }


        //// one - to - one required
        //// place the FK of these into the respective models, not nesc. here
        //public FullName FullName { get; set; }
        //public RecordCollector RecordCollector { get; set; }
        //public ContactInfo ContactInfo { get; set; }
        

//        public ICollection<Person> People { get; set; }
 //           = new List<Person>();



        ///// <summary>
        ///// FK to ApplicationUser
        ///// </summary>
        ////public string ApplicationUserId { get; set; }
        /////<summary>
        ///// Navigation property from Person to ApplicationUser
        /////</summary>
        //public ApplicationUser ApplicationUser { get; set; }


        // public ContactInfo ContactInfo { get; set; }
        // public PersonName PersonName { get; set; }
        // public RecordCollector RecordCollector { get; set; }

    }
}
