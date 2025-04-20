using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDexTest_01_MVC.Services
{
    public interface IApiCallerService
    {
        /// <summary>
        /// makes a call to the Web Api
        /// </summary>
        /// <param name="httpMethod">GET, POST, PUT, etc</param>
        /// <param name="url">he url that needs to be called</param>
        /// <param name="bodyContent">in case we wanted to pass some data in the request body. Default is null</param>
        /// <param name="acceptHeader">defaults to <code>application/json</code></param>
        /// <param name="authScheme">used in case we want to provide auth token e.g bearer. Default is null</param>
        /// <param name="authToken">auth token, default is null</param>
        /// <param name="extraHeaders">in case we wants to add any extra headers</param>
        /// <returns></returns>
        Task<HttpResponseMessage> MakeHttpCallAsync(
            HttpMethod httpMethod,      // GET, POST, PUT, etc
            string url,
            object bodyContent = null,
            string acceptHeader = "application/json",
            string authScheme = null,
            string authToken = null,
            Dictionary<string, string> extraHeaders = null);
    }
}