using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NetDexTest_01.Models.Entities
{

    /// <summary>
    /// 
    /// </summary>
    [Table("RecordCollector")]
    public class RecordCollector 
    {
        [Key]
        public string Id { get; set; }

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
