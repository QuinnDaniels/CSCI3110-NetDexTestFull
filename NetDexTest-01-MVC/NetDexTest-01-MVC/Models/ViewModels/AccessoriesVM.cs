using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Security.Cryptography.Pkcs;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NetDexTest_01_MVC.Models.Entities;


namespace NetDexTest_01_MVC.Models.ViewModels
{


        public class SocialMediaVMResponse : SocialMediaVM
    {
        public HttpStatusCode Status { get; set; }
        public string? Title { get; set; } = "default title";
        public string? Message { get; set; } = "default message";

    }
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


    /*-----------------------------*/

    public class EntryItemVMResponse : EntryItemVM
    {
        public HttpStatusCode Status { get; set; }
        public string? Title { get; set; } = "default title";
        public string? Message { get; set; } = "default message";

    }
    public class EntryItemVM
    {

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
        public DateTime LogTimestamp { get; set; }// = DateTime.Now;
        public Int64 RecordCollectorId { get; set; }

    }


    // NOTE - later: create a /Models/DTOs/EntryItemDTOs.cs file
    public class EntryItemDTO
    {
        public Int64? EntryItemId { get; set; } // HACK - changed to nullable for Create view
        public Int64? RecordCollectorId { get; set; } // HACK - changed to nullable for Create view
        public int? PersonId { get; set; } // HACK - changed to nullable for Create view
        public int? DexHolderId { get; set; } // HACK - changed to nullable for Create view
        public string? PersonNickname { get; set; } // HACK - changed to nullable for Create view
        public string? ApplicationUserEmail { get; set; } // HACK - changed to nullable for Create view
        public string? ApplicationUserName { get; set; } // HACK - changed to nullable for Create view
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