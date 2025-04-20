/**
 * 
 * https://code-maze.com/authentication-aspnetcore-jwt-1/
 */
using System.ComponentModel.DataAnnotations;

namespace NetDexTest_01.Models
{
    public class LoginModel
    {

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Username")]
        public string? Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        // https://memorycrypt.hashnode.dev/create-a-web-api-with-jwt-authentication-and-aspnet-core-identity

    }


}

