using NetDexTest_01_MVC.Models.ViewModels;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace NetDexTest_01_MVC.Services
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly JsonSerializerOptions _jsonOptions;

        public SocialMediaService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<SocialMediaVM>> GetAllAsync()
        {
            var response = await _client.GetAsync("https://localhost:7134/api/socialmedia/transfer/all");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<SocialMediaVM>>(_jsonOptions);
        }

        public async Task<SocialMediaVM?> GetByIdAsync(long id)
        {
            var response = await _client.GetAsync($"https://localhost:7134/api/socialmedia/transfer/one/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SocialMediaVM>(_jsonOptions);
        }

        public async Task<bool> CreateAsync(SocialMediaVM model)
        {
            var form = new MultipartFormDataContent
            {
                { new StringContent(model.ContactInfoId.ToString()), "ContactInfoId" },
                { new StringContent(model.CategoryField ?? ""), "CategoryField" },
                { new StringContent(model.SocialHandle ?? ""), "SocialHandle" }
            };

            var response = await _client.PostAsync("https://localhost:7134/api/socialmedia/create", form);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(SocialMediaVM model)
        {
            var form = new MultipartFormDataContent
            {
                { new StringContent(model.Id.ToString()), "Id" },
                { new StringContent(model.CategoryField ?? ""), "CategoryField" },
                { new StringContent(model.SocialHandle ?? ""), "SocialHandle" }
            };

            var response = await _client.PutAsync("https://localhost:7134/api/socialmedia/put", form);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var response = await _client.DeleteAsync($"https://localhost:7134/api/socialmedia/delete/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
