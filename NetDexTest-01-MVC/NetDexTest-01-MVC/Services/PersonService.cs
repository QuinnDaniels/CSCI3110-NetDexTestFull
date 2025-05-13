using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NetDexTest_01_MVC.Models;
using NetDexTest_01_MVC.Models.ViewModels;
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
        public async Task<PeopleResponse> CreatePersonAsync(NewPersonVM person)
        {
            var token = _authService.GetSavedClaims().AuthToken;

            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Post,
                url: _url,
                bodyContent: person
            //authScheme: "bearer",
            //authToken: token
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


        public async Task<DexHolderMiddleVM?> GetDexHolderMiddleVMAsync(string input)
        {
            var url = "https://localhost:7134/api/user/find";//_config["apiService:dexViewUrl"];
            url = url + $"/{input}";
            //var url = _config["apiService:userLogin2Url"];
            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Get,
                url: url
                //, bodyContent: request
                );
            //LoginResponse loginResponse = new LoginResponse();
            DexHolderMiddleVM? dexResponse = new ();

            //if call was successful
            if (httpResponse.StatusCode == HttpStatusCode.OK) //httpResponse.IsSuccessStatusCode
            {
                dexResponse = await httpResponse.Content.ReadFromJsonAsync<DexHolderMiddleVM>();
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE-----------------\n\n{dexResponse.ToString}\n\n\n-----------------------");

                return dexResponse;//.AdminUserVMs;
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


        public async Task<PersonPlusDexListVM?> GetPersonPlusDexListVMAsync(string input, string criteria)
        {
            var url = "https://localhost:7134/api/people";// _config["apiService:peopleUrl"];
            url = url + $"/retrieveRequestpath/{input}/{criteria}"; //criteria null?
            //var url = _config["apiService:userLogin2Url"];
            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Get,
                url: url
                //, bodyContent: request
                );
            //LoginResponse loginResponse = new LoginResponse();
            //PersonPlusDexListVMRes? personPlusResponse = new();
            PersonPlusDexListVM? personPlus = new();


            //if call was successful
            if (httpResponse.StatusCode == HttpStatusCode.OK) //httpResponse.IsSuccessStatusCode
            {
                personPlus = await httpResponse.Content.ReadFromJsonAsync<PersonPlusDexListVM>();
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE--PersonPlus-----\n\n{personPlus?.ToString()??" null!!! "}\n\n\n-----------------------");
                //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE--PersonPlus-----\n\n{personPlusResponse.ToString}\n\n\n-----------------------");

                return personPlus;//.AdminUserVMs;
                //userResponse.Status = httpResponse.StatusCode;

            }
            else
            {
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE----is success?--\n\n{httpResponse.IsSuccessStatusCode}\n\n\n-----------------------");
                //else if login failed, map the error message
                var errMessage = await httpResponse.Content.ReadAsStringAsync();
                //userResponses.Status = httpResponse.StatusCode;
                //userResponses.Message = errMessage;
                
                
                await Console.Out.WriteLineAsync($"\n\n\t\tTrying one more time......\n\n");
                personPlus = await httpResponse.Content.ReadFromJsonAsync<PersonPlusDexListVM>();
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE--PersonPlus-----\n\n{personPlus?.ToString() ?? " null!!! "}\n\n\n-----------------------");
                
                if(personPlus != null) return personPlus;

                await Console.Out.WriteLineAsync($"\n\n\t\tOne more time for the hell of it......\n\n");
                personPlus = await httpResponse.Content.ReadFromJsonAsync<PersonPlusDexListVM>();
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE--PersonPlus-----\n\n{personPlus?.ToString() ?? " null!!! "}\n\n\n-----------------------");

                if (personPlus != null) return personPlus;

                return null;
                //return userResponses;
            }
        }

        public async Task<PersonDexListVM?> GetPersonDexListVMAsync(string input, string criteria)
        {
            var url = "https://localhost:7134/api/people";// _config["apiService:peopleUrl"];
            url = url + $"/retrieveRequestpath/{input}/{criteria}"; //criteria null?
            //var url = _config["apiService:userLogin2Url"];
            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Get,
                url: url
                //, bodyContent: request
                );
            //LoginResponse loginResponse = new LoginResponse();
            //PersonPlusDexListVMRes? personPlusResponse = new();
            PersonDexListVM? personPlus = new();


            //if call was successful
            if (httpResponse.StatusCode == HttpStatusCode.OK) //httpResponse.IsSuccessStatusCode
            {
                personPlus = await httpResponse.Content.ReadFromJsonAsync<PersonDexListVM>();
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE--PersonDexList-----\n\n{personPlus?.ToString() ?? " null!!! "}\n\n\n-----------------------");
                //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE--PersonDexList-----\n\n{personPlusResponse.ToString}\n\n\n-----------------------");

                return personPlus;//.AdminUserVMs;
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



        public async Task<IEnumerable<RelationshipVM>> GetRelationshipsForUserAsync(string userInput)
        {
            var url = "https://localhost:7134/api/people";
            string endpoint = $"{url}/api/people/retrieveRelations/User/{userInput}";
            
            
            
            //var result = await _apiService.GetFromApiAsync<List<RelationshipVM>>(endpoint);
            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Get,
                url: endpoint
                //, bodyContent: request
            );

            List<RelationshipVM> relList = new();
            //RelationshipRequest request = new RelationshipRequest();



            if (httpResponse.StatusCode == HttpStatusCode.OK) //httpResponse.IsSuccessStatusCode
            {
                relList = await httpResponse.Content.ReadFromJsonAsync<ICollection<RelationshipVM>>();
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE--PersonDexList-----\n\n{personPlus?.ToString() ?? " null!!! "}\n\n\n-----------------------");
                //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE--PersonDexList-----\n\n{personPlusResponse.ToString}\n\n\n-----------------------");

                return personPlus;//.AdminUserVMs;
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




            return result ?? new List<RelationshipVM>();
        
        
        
        
        
        }

        public async Task<HttpResponseMessage> CreateRelationshipAsync(RelationshipRequest request)
        {
            var url = "https://localhost:7134/api/people";
            string endpoint = $"{url}/api/people/relations/create";
            //var response = await _apiService.PostToApiAsync(endpoint, request);

            var httpResponse = await _apiService.MakeHttpCallAsync(
                httpMethod: HttpMethod.Get,
                url: endpoint
                , bodyContent: request
                );


            if (httpResponse.StatusCode == HttpStatusCode.OK) //httpResponse.IsSuccessStatusCode
            {
                personPlus = await httpResponse.Content.ReadFromJsonAsync<PersonDexListVM>();
                await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE--PersonDexList-----\n\n{personPlus?.ToString() ?? " null!!! "}\n\n\n-----------------------");
                //await Console.Out.WriteLineAsync($"\n\n\n--------HTTP RESPONSE--PersonDexList-----\n\n{personPlusResponse.ToString}\n\n\n-----------------------");

                return personPlus;//.AdminUserVMs;
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





            return response;
        }










    }


}