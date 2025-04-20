using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace NetDexTest_01_MVC.Models
{
    public class RevokeRequestModel
    {
        [Required]
        public string RefreshToken { get; set; }

    }
}