using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDexTest_01_MVC.Services
{

    public class Claims
    {
        public string Email { get; set; }

        public string AuthToken { get; set; }

        public string RefreshToken { get; set; }

    }
}
