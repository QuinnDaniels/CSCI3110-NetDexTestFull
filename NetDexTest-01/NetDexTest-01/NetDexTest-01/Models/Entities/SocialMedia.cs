using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetDexTest_01.Models.Entities
{
    [Table("SocialMedia")]
    public class SocialMedia
    {
        [Key]
        public Int64 Id { get; set; }
        [StringLength(120)]
        public string CategoryField { get; set; } = string.Empty;
        public string? SocialHandle { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime LogTimestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// FK to ContactInfo
        /// </summary>
        public Int64 ContactInfoId { get; set; }
        ///<summary>
        /// Navigation property from SocialMedia to ContactInfo
        ///</summary>
        public virtual ContactInfo ContactInfo { get; set; }


        public SocialMediaVM getSocialMediaVM()
        {
            return new SocialMediaVM
            {
                Id = this.Id,
                CategoryField = this.CategoryField,
                SocialHandle = this.SocialHandle,
                LogTimestamp = this.LogTimestamp,
                ContactInfoId = this.ContactInfoId
            };



        }
    }
}
