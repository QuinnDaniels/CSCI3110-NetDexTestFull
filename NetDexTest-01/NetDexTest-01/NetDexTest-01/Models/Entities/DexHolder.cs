using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NetDexTest_01.Models.Entities
{

    /// <summary>
    /// A Dex User (or Holder, to differentiate from ASP.NET User).
    /// </summary>
    [Table("DexHolder")]
    public class DexHolder 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string ApplicationUserId { get; set; }  // FK to ApplicationUser.Id

        [Required]
        [StringLength(256)]
        public string ApplicationUserName { get; set; }// = null!; // FK to ApplicationUser.UserName


        [StringLength(256)]
        public string? FirstName { get; set; }// = String.Empty;
        [StringLength(256)]
        public string? MiddleName { get; set; }// = String.Empty;
        [StringLength(256)]
        public string? LastName { get; set; }// = String.Empty;

        
        [StringLength(120)]
        public string? Gender { get; set; } = String.Empty;
        [StringLength(25)]
        public string? Pronouns { get; set; } = String.Empty;

        // User -- navigation
        //      // unique dexname?
        // People -- collection

        //public ICollection<Person> People { get; set; }
       //     = new List<Person>();

        // Navigation property
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }// = null!;

        [NotMapped]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; } = DateTime.Now;


    }
}
