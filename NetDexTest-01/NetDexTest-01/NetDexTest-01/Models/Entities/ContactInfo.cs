using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NetDexTest_01.Models.Entities
{

    /// <summary>
    /// A Dex User (or Holder, to differentiate from ASP.NET User).
    /// </summary>
    [Table("ContactInfo")]
    public class ContactInfo 
    {
        [Key]
        public string Id { get; set; }

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
    }
}
