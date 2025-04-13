using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NetDexTest_01.Models.Entities
{
    [Index(nameof(Nickname), IsUnique = true)]
    public class Person
    {
        public int Id { get; set; }

        [Required]
        
        [StringLength(256)]
        public string Nickname { get; set; }



        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; } = DateTime.Now;

        [StringLength(120)]
        public string? Gender { get; set; } = String.Empty;
        [StringLength(25)]
        public string? Pronouns { get; set; } = String.Empty;

        public int Ratings { get; set; } = 0;

        public bool Favorite { get; set; } = false;

        public DexHolder DexHolder { get; set; }

        // public ContactInfo ContactInfo { get; set; }
        // public PersonName PersonName { get; set; }
        // public RecordCollector RecordCollector { get; set; }

    }
}
