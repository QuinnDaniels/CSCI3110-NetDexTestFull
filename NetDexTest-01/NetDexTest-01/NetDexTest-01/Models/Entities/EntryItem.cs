using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetDexTest_01.Models.Entities
{
    [Table("EntryItem")]
    public class EntryItem
    {
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// A short title to easily identify an Entry. Nullable, because on
        /// the frontend, if null, it should be replaced with a breif preview of flavortext 
        /// </summary>
        [StringLength(240)]
        public string? ShortTitle { get; set; }
        [StringLength(456)]
        public string FlavorText { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        public DateTime LogTimestamp { get; set; } = DateTime.Now;



        /// <summary>
        /// FK to RecordCollector
        /// </summary>
        public Int64 RecordCollectorId { get; set; }
        ///<summary>
        /// Navigation property from EntryItem to RecordCollector
        ///</summary>
        public virtual RecordCollector RecordCollector { get; set; }

    }
}
