using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.Pkcs;


namespace NetDexTest_01_MVC.Models.Entities
{
    public class Person
    {
        public Person() { }
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


        [Key]
        public int Id { get; set; }

        //[Required]

        [StringLength(256)]
        public string Nickname { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(120)]
        public string? Gender { get; set; }
        [StringLength(25)]
        public string? Pronouns { get; set; }

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



    }
}
