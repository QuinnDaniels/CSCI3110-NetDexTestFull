using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetDexTest_01_MVC.Models.ViewModels;
namespace NetDexTest_01_MVC.Models.Entities
{
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


        public EntryItemVM getEntryItemVM()
        {
            return new EntryItemVM
            {
                Id = this.Id,
                ShortTitle = this.ShortTitle,
                FlavorText = this.FlavorText,
                LogTimestamp = this.LogTimestamp,
                RecordCollectorId = this.RecordCollectorId
            };

        }

    }
}
