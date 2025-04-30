using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetDexTest_01.Models.Entities
{
    [Table("Person")]
    [Index(nameof(Nickname), nameof(DexHolderId), IsUnique = true)]
    public partial class Person
    {
        public Person() { }

        private Person(ApplicationDbContext context)
        {
            Context = context;
        }

        private ApplicationDbContext Context { get; set; }


        public Person(string nickName, DexHolder dexHolder)
        {
            Nickname = nickName;
            DexHolder = dexHolder;
            FullName = new FullName();
            RecordCollector = new RecordCollector()
            {
                EntryItems = new List<EntryItem>
                {
                    new EntryItem { ShortTitle = "Example", FlavorText = $"This is an example entry! It was automatically created on [ ${DateTime.Now} ]!" }
                }
            };
            ContactInfo = new ContactInfo()
            {
                SocialMedias = new List<SocialMedia>
                    {
                        new SocialMedia {  CategoryField = "example category", SocialHandle = "@NetDexTest-01_Example_SocialHandle" }
                    }
            };

        }

        public Person(string nickName, DexHolder dexHolder, int rating)
        {
            Nickname = nickName;
            DexHolder = dexHolder;
            FullName = new FullName();
            Rating = rating;
            RecordCollector = new RecordCollector()
            {
                EntryItems = new List<EntryItem>
                {
                    new EntryItem { ShortTitle = "Example", FlavorText = $"This is an example entry! It was automatically created on [ ${DateTime.Now} ]!" }
                }
            };
            ContactInfo = new ContactInfo()
            {
                SocialMedias = new List<SocialMedia>
                    {
                        new SocialMedia {  CategoryField = "example category", SocialHandle = "@NetDexTest-01_Example_SocialHandle" }
                    }
            };

        }



        [Key]
        public int Id { get; set; }

        //[Required]

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
        public virtual RecordCollector RecordCollector { get; set; } //  = new RecordCollector();  
        public virtual ContactInfo ContactInfo { get; set; }
        



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
