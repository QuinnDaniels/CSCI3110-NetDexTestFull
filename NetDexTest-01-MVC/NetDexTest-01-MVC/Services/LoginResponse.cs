using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NetDexTest_01_MVC.Services
{
    /// <summary>
    /// <para>
    /// added to store details of whether the Web Api request succeeded or failed, along with the access and refresh tokens received from the web api. 
    /// </para>
    /// <para>
    /// <seealso href="https://memorycrypt.hashnode.dev/net-mvc-app-calling-web-api-for-authentication"/>
    /// </para>
    /// </summary>
    public class LoginResponse
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }


    }
}