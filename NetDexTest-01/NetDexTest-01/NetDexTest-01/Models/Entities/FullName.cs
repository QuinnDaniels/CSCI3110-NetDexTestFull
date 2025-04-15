using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NetDexTest_01.Models.Entities
{

    /// <summary>
    /// A Dex User (or Holder, to differentiate from ASP.NET User).
    /// </summary>
    [Table("FullName")]
    public class FullName 
    {
        [Key]
        public Int64 Id { get; set; }


        [StringLength(120)]
        public string? NameFirst { get; set; }
        [StringLength(120)]
        public string? NameMiddle { get; set; }
        [StringLength(120)]
        public string? NameLast { get; set; }
        [StringLength(120)]
        public string? PhNameFirst { get; set; }
        [StringLength(120)]
        public string? PhNameMiddle { get; set; }
        [StringLength(120)]
        public string? PhNameLast { get; set; }

        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

    }



    // LATER: research viability of converting into a ComplexType
    // EF Core Complex Type https://www.learnentityframeworkcore.com/model/complex-type
}
