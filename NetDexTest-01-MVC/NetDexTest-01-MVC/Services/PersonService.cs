using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NetDexTest_01_MVC.Models;
using NetDexTest_01_MVC.Models.Entities;
using NetDexTest_01_MVC.Models.Authentication;


namespace NetDexTest_01_MVC.Services
{

    public class PersonService : IPersonService
    {
        private IConfiguration _config;
        private IAuthService _authService;
        private IApiCallerService _apiService;
        private string _url;
        public PersonService(IConfiguration config, IApiCallerService apiService, IAuthService authService)
        {
            _config = config;
            _apiService = apiService;
            _authService = authService;
            _url = _config["apiService:peopleUrl"];
        }

        public async Task<PeopleResponse> CreatePersonAsync(Person person)
        {
            var token = _authService.GetSavedClaims().AuthToken;

            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Post,
                url: _url,
                bodyContent: person,
                authScheme: "bearer",
                authToken: token
            );
            PeopleResponse response = new PeopleResponse();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var createdPerson = await httpResponse.Content.ReadFromJsonAsync<Person>();
                response.Status = httpResponse.StatusCode;
                response.People = new List<Person> { createdPerson };
            }
            else
            {
                response.Status = httpResponse.StatusCode;
                response.Message = await httpResponse.Content.ReadAsStringAsync();
            }
            return response;
        }
    }

    
}