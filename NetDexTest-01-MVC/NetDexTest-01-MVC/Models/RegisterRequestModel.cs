using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetDexTest_01_MVC.Models
{
    /// <summary>
    /// RegisterRequest
    /// </summary>
    public class RegisterRequestModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        ///     TODO adding for extra field in ViewModel
        /// </summary>
        [Required]
        [Display(Name = "Username")]
        [StringLength(50, MinimumLength = 1)]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }

        // [Required]
        // [StringLength(50, MinimumLength = 3)]
        // [Display(Name = "Display Name")]
        // public string DisplayName { get; set; }

        /*---------------------------------*/

        //[Required]
        [Display(Name = "FirstName")]
        public string? FirstName { get; set; }

        [Display(Name = "MiddleName")]
        public string? MiddleName { get; set; }


        [Display(Name = "LastName")]
        public string? LastName { get; set; }

        [Display(Name = "Gender")]
        public string? Gender { get; set; }


        [Display(Name = "Pronouns")]
        public string? Pronouns { get; set; }



    }
}