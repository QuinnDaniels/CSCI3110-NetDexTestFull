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

        [Display(Name = "Password")]
        public string? Password { get; set; }


        }


}

