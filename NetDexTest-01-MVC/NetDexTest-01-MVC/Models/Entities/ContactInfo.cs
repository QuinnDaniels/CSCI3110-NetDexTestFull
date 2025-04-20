using NetDexTest_01_MVC.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NetDexTest_01_MVC.Models.Entities
{

    /// <summary>
    /// Collector table for various forms of contact information entities.
    /// </summary>
    public class ContactInfo 
    {
        public ContactInfo() { }


        // ------------



        [Key]
        public Int64 Id { get; set; }

        [StringLength(456)]
        public string NoteText { get; set; } = string.Empty;
        
        // replace this?
        [NotMapped]
        [DataType(DataType.DateTime)]
        public DateTime? LastUpdated { get; set; } = DateTime.Now;
        /// <summary>
        /// FK to Person
        /// </summary>
        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        public ICollection<SocialMedia> SocialMedias { get; set; }
            = new List<SocialMedia>();

        /// <summary>
        /// Count of the number of SocialMedia records that are associated with the ContactInfo
        /// </summary>
        /// <inheritdoc cref="NetDexTest_01.Models.Entities.DexHolder.PeopleCount"/>
        public int SocialMediasCount { get; set; }

    }
}
