using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NetDexTest_01.Models.Entities
{

    /// <summary>
    /// Collector table for various forms of "RecordItems" entites
    /// </summary>
    [Table("RecordCollector")]
    public class RecordCollector 
    {

        public RecordCollector() { }

        private RecordCollector(ApplicationDbContext context)
        {
            Context = context;
        }

        private ApplicationDbContext Context { get; set; }

        // ------------


        [Key]
        public Int64 Id { get; set; }

        [StringLength(456)]
        [NotMapped]
        public string? NoteText { get; set; }
        [NotMapped]
        [DataType(DataType.Date)]
        public DateTime? LastUpdated { get; set; } = DateTime.Now;
        /// <summary>
        /// FK to Person
        /// </summary>
        public int PersonId { get; set; }
        /// <summary>
        /// Navigation property to Person
        /// </summary>
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        /// <summary>
        /// List of EntryItem objects
        /// </summary>
        public ICollection<EntryItem> EntryItems { get; set; }
            = new List<EntryItem>();


        /// <summary>
        /// Count of the number of SocialMedia records that are associated with the ContactInfo
        /// </summary>
        /// <inheritdoc cref="NetDexTest_01.Models.Entities.DexHolder.PeopleCount"/>
        public int EntryItemsCount
        => EntryItems?.Count
            ?? Context?.Set<EntryItem>().Count(p => Id == EF.Property<Int64?>(p, "RecordCollectorId"))
            ?? 0;



        ///// <summary>
        ///// List of EventItem objects
        ///// </summary>
        //public ICollection<EventItem> EventItems { get; set; }
        //    = new List<EventItem>();

        ///// <summary>
        ///// List of QuoteItem objects
        ///// </summary>
        //public ICollection<QuoteItem> QuoteItems { get; set; }
        //    = new List<QuoteItem>();

    }
}
