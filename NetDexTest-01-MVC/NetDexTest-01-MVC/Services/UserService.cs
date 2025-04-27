using NetDexTest_01_MVC.Models.ViewModels;
using NuGet.Protocol.Plugins;
using System.Net;

namespace NetDexTest_01_MVC.Services
{
    public class UserService : IUserService
    {

        private IApiCallerService _apiService;
        private IHttpContextAccessor _contextAccessor;
        private IConfiguration _config;
        private readonly IUserSessionService _userSessionService;


        public UserService(
            IApiCallerService apiService, IHttpContextAccessor contextAccessor,
            IConfiguration config, IUserSessionService userSessionService)
        {
            _apiService = apiService;
            _contextAccessor = contextAccessor;
            _config = config;
            _userSessionService = userSessionService;
        }


        public async Task<ICollection<AdminUserVM>?> GetAllUsersAdminAsync()
        {
            var url = _config["apiService:userAdminAllUrl"];
            //var url = _config["apiService:userLogin2Url"];
            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Get,
                url: url
                //, bodyContent: request
                );
            //LoginResponse loginResponse = new LoginResponse();
            ICollection<AdminUserVM>? userResponses = new List<AdminUserVM>();

            //if call was successful
            if (httpResponse.StatusCode == HttpStatusCode.OK) //httpResponse.IsSuccessStatusCode
            {
                userResponses = await httpResponse.Content.ReadFromJsonAsync<ICollection<AdminUserVM>>();
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{userResponses.ToString}\n\n\n-----------------------");

                return userResponses;//.AdminUserVMs;
                //userResponse.Status = httpResponse.StatusCode;

            }
            else
            {
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----is success?--\n\n{httpResponse.IsSuccessStatusCode}\n\n\n-----------------------");
                //else if login failed, map the error message
                var errMessage = await httpResponse.Content.ReadAsStringAsync();
                //userResponses.Status = httpResponse.StatusCode;
                //userResponses.Message = errMessage;
                return null;
                //return userResponses;
            }
        }

        //public async Task<AdminUserRootResponse>> Authorized_GetAllUsersAdminAsync()
        //{

        //    if(await _userSessionService.HasAnyRoleAsync("Admin", "Administrator", "Moderator"))
        //    {
        //        var url = _config["apiService:userAdminAllUrl"];
        //        //var url = _config["apiService:userLogin2Url"];
        //        var httpResponse = await _apiService.MakeHttpCallAsync(
        //            httpMethod: HttpMethod.Get,
        //            url: url
        //            //, bodyContent: request
        //            );
        //        //LoginResponse loginResponse = new LoginResponse();
        //        AdminUserVMResponse userResponse = new AdminUserVMResponse();

        //        //if call was successful
        //        if (httpResponse.StatusCode == HttpStatusCode.OK) //httpResponse.IsSuccessStatusCode
        //        {
        //            userResponse = await httpResponse.Content.ReadFromJsonAsync<AdminUserVMResponse>();
        //            await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{userResponse.ToString}\n\n\n-----------------------");

        //            //userResponse.Status = httpResponse.StatusCode;

        //        }
        //        else
        //        {
        //            await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----is success?--\n\n{httpResponse.IsSuccessStatusCode}\n\n\n-----------------------");
        //            //else if login failed, map the error message
        //            var errMessage = await httpResponse.Content.ReadAsStringAsync();
        //            userResponse.Status = httpResponse.StatusCode;
        //            userResponse.Message = errMessage;
        //        }
        //        return userResponse;
        //    }
        //    else { return null; }
        //}


    }


}

