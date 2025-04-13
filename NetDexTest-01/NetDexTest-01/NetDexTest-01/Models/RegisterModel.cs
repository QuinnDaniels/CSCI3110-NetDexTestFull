using System.ComponentModel.DataAnnotations;

namespace NetDexTest_01.Models
{
    public class RegisterModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        /// <summary>
        ///     TODO adding for extra field in ViewModel
        /// </summary>
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }



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

