using System.ComponentModel.DataAnnotations;

namespace NetDexTest_01.Models
{
    public class AddRoleModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }


        //[Required]
        //[Display(Name = "Username")]
        //public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }



    }


}

