using NetDexTest_01_MVC.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace NetDexTest_01_MVC.Models.Entities
{
    // TODO https://stackoverflow.com/a/65807395
    public partial class Person
    {
        public List<PersonPerson> PersonChildren { get; set; } = new List<PersonPerson>();
        public List<PersonPerson> PersonParents { get; set; } = new List<PersonPerson>();
    }

    public partial class PersonPerson
    {
        [Key]
        public int Id { get; set; }

        public int? PersonChildId { get; set; }
        [ForeignKey("PersonChildId")]
        public virtual Person PersonChild { get; set; }

        /// <summary>
        /// Describes the relationship between the PersonParent and PersonChild
        /// </summary>
        public string RelationshipDescription { get; set; } = string.Empty;
        [NotMapped]
        [DataType(DataType.DateTime)]
        public DateTime? LastUpdated { get; set; } = DateTime.Now;

        public int? PersonParentId { get; set; }
        [ForeignKey("PersonParentId")]
        public virtual Person PersonParent { get; set; }

        public PersonPerson() { }
        public PersonPerson(Person personParent, Person personChild)
        {
            if (personParent.DexHolder == personChild.DexHolder)
            {
                PersonParent = personParent;
                PersonChild = personChild;
            }

        }

        public PersonPerson(Person personParent, Person personChild, string Descriptor)
        {
            if (personParent.DexHolder == personChild.DexHolder)
            {
                PersonParent = personParent;
                PersonChild = personChild;
                RelationshipDescription =  Descriptor;
            }
        }


        public PersonPerson(PersonPerson newEntry, string Descriptor)
        {
            if (newEntry.RelationshipDescription != Descriptor)
            {
                PersonParent = newEntry.PersonParent;
                PersonChild = newEntry.PersonChild;
                RelationshipDescription = Descriptor;
            }

        }



    }






    //[Table("TagItem")]
    //[Index(nameof(TagText), IsUnique = true)]
    //public class TagItem
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    [StringLength(10)]
    //    public string TagText { get; set; } = String.Empty;

    //    public ICollection<StudentCourseGrade> CourseGrades { get; set; }
    //    = new List<StudentCourseGrade>();
    //}












}
