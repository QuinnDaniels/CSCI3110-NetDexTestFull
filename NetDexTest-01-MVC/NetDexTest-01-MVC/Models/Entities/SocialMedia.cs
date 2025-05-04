using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetDexTest_01_MVC.Models.ViewModels;
namespace NetDexTest_01_MVC.Models.Entities
{
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
