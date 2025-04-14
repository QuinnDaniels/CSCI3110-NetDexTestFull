using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetDexTest_01.Models.Entities
{
    [Table("Person")]
    [Index(nameof(Nickname), IsUnique = true)]
    public class Person
    {



        [Key]
        public int Id { get; set; }

        [Required]

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
        /// <summary>
        /// FK to DexHolderId
        /// </summary>
        public int DexHolderId { get; set; } 
        /// <summary>
        /// Navigation property from Person to DexHolder
        /// </summary>
        public virtual DexHolder DexHolder { get; set; }


        //// one - to - one required
        //// place the FK of these into the respective models, not nesc. here
        public virtual FullName FullName { get; set; }
        public RecordCollector RecordCollector { get; set; } //  = new RecordCollector();  
        public ContactInfo ContactInfo { get; set; }
        



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
