using NetDexTest_01_MVC.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NetDexTest_01_MVC.Models.Entities
{

    /// <summary>
    /// A Dex User (or Holder, to differentiate from ASP.NET User).
    /// </summary>
    public class DexHolder 
    {
        public DexHolder() { }

        // ------------

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

        public ICollection<Person> People { get; set; }
            = new List<Person>();

        // Navigation property
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }// = null!;

        /// <summary>
        /// <para>Count of the number of People that are associated with the DexHolder</para>
        /// </summary>
        /// <remarks>
        /// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/contructors">Source - "Injecting Services"</see>
        /// </remarks>
        public int PeopleCount { get; set; }
        //=> People?.Count
        //        ?? Context?.Set<Person>().Count(p => Id == EF.Property<int?>(p, "DexHolderId"))
        //        ?? 0;

        [NotMapped]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; } = DateTime.Now;


    }
}
