using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;
using System.ComponentModel.DataAnnotations;



namespace NetDexTest_01.Models.ViewModels
{
    //public class AccessoriesVMs
    //{
    //}

    public class SocialMediaVM
    {

        public Int64 Id { get; set; }
        [StringLength(120)]
        public string CategoryField { get; set; } = string.Empty;
        public string? SocialHandle { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime LogTimestamp { get; set; }// = DateTime.Now;
        public Int64 ContactInfoId { get; set; }

    }


    public class SocialMediaVMNotes : SocialMediaVM
    {
        public string NoteText { get; set; } = string.Empty;

    }


    public class EntryItemVM
    {

        public Int64 Id { get; set; } = 0;
        /// <summary>
        /// A short title to easily identify an Entry. Nullable, because on
        /// the frontend, if null, it should be replaced with a breif preview of flavortext 
        /// </summary>
        [StringLength(240)]
        public string? ShortTitle { get; set; }
        [StringLength(456)]
        public string FlavorText { get; set; } = string.Empty;
        [DataType(DataType.DateTime)]
        public DateTime LogTimestamp { get; set; }// = DateTime.Now;
        public Int64 RecordCollectorId { get; set; }

    
    }



    // PORTME
    // NOTE - later: create a /Models/DTOs/EntryItemDTOs.cs file
    public class EntryItemDTO
    {
        public Int64 EntryItemId { get; set; }
        public Int64 RecordCollectorId { get; set; }
        public int PersonId { get; set; }
        public int DexHolderId { get; set; }
        public string PersonNickname { get; set; }
        public string ApplicationUserEmail { get; set; }
        public string ApplicationUserName { get; set; }
        public string? ShortTitle { get; set; }
        public string? FlavorText { get; set; }
        public DateTime LogTimestamp { get; set; }
    }


    public class SocialMediaDTO
    {
        public Int64 SocialMediaId { get; set; }
        public Int64 ContactInfoId { get; set; }
        public int PersonId { get; set; }
        public int DexHolderId { get; set; }
        public string PersonNickname { get; set; } = string.Empty;
        public string ApplicationUserEmail { get; set; } = string.Empty;
        public string ApplicationUserName { get; set; } = string.Empty;
        public string ContactInfoNoteText { get; set; } = string.Empty;
        public string CategoryField { get; set; } = string.Empty;
        public string SocialHandle { get; set; } = string.Empty;
        public DateTime LogTimestamp { get; set; }
    }




}
