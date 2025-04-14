using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NetDexTest_01.Models.Entities
{

    /// <summary>
    /// Collector table for various forms of contact information entities.
    /// </summary>
    [Table("ContactInfo")]
    public class ContactInfo 
    {
        public ContactInfo() { }

        private ContactInfo(ApplicationDbContext context)
        {
            Context = context;
        }

        private ApplicationDbContext Context { get; set; }

        // ------------



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

        /// <summary>
        /// Count of the number of SocialMedia records that are associated with the ContactInfo
        /// </summary>
        /// <inheritdoc cref="NetDexTest_01.Models.Entities.DexHolder.PeopleCount"/>
        public int SocialMediasCount
        => SocialMedias?.Count
            ?? Context?.Set<SocialMedia>().Count(p => Id == EF.Property<string?>(p, "ContactInfoId"))
            ?? 0;

    }
}
