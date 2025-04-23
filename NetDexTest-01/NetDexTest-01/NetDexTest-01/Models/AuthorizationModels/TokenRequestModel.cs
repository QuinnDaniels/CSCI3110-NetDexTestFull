using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;

namespace NetDexTest_01.Models
{
    public class TokenRequestModel
    {
        public TokenRequestModel() { }

        public TokenRequestModel(string em, string pass)
        {
            Password = pass;
            Email = em;
        }


        [Required]
        //[EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        //[Required]
        //[Display(Name = "Username")]
        //public string Username { get; set; }


        [Required]
        [Display(Name = "Password")]
        public virtual string Password { get; set; }


    }


}

