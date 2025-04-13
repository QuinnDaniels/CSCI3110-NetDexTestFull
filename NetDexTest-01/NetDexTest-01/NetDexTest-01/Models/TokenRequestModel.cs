using System.ComponentModel.DataAnnotations;

namespace NetDexTest_01.Models
{
    public class TokenRequestModel
    {
        [Required]
        //[EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        //[Required]
        //[Display(Name = "Username")]
        //public string Username { get; set; }


        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }


        }


}

