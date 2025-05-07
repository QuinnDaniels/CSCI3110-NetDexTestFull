using NetDexTest_01_MVC.Models.ViewModels;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace NetDexTest_01_MVC.Services
{
    public class EntryItemService : IEntryItemService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly JsonSerializerOptions _jsonOptions;

        public EntryItemService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<EntryItemVM>> GetAllAsync()
        {
            var response = await _client.GetAsync("https://localhost:7134/api/entry/transfer/all");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<EntryItemVM>>(_jsonOptions);
        }

        public async Task<EntryItemVM?> GetByIdAsync(long id)
        {
            var response = await _client.GetAsync($"https://localhost:7134/api/entry/transfer/one/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<EntryItemVM>(_jsonOptions);
        }

        public async Task<bool> CreateAsync(EntryItemVM model)
        {
            var form = new MultipartFormDataContent
            {
                { new StringContent(model.RecordCollectorId.ToString()), "RecordCollectorId" },
                { new StringContent(model.ShortTitle ?? ""), "ShortTitle" },
                { new StringContent(model.FlavorText ?? ""), "FlavorText" }
            };

            var response = await _client.PostAsync("https://localhost:7134/api/entry/create", form);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(EntryItemVM model)
        {
            var form = new MultipartFormDataContent
            {
                { new StringContent(model.Id.ToString()), "Id" },
                { new StringContent(model.ShortTitle ?? ""), "ShortTitle" },
                { new StringContent(model.FlavorText ?? ""), "FlavorText" }
            };

            var response = await _client.PutAsync("https://localhost:7134/api/entry/put", form);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var response = await _client.DeleteAsync($"https://localhost:7134/api/entry/delete/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
