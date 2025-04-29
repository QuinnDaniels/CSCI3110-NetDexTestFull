using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace NetDexTest_01_MVC.Models.Authentication
{
    public class LoginRequestModel
    {
        //[Required]

        public string? Input { get; set; }

        public string? Username { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        //public string? Token { get; set; }

    }
}