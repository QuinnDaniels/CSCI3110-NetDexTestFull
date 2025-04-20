using System.ComponentModel.DataAnnotations;

namespace NetDexTest_01.Models
{
    public class AuthenticationModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }

    }


}

