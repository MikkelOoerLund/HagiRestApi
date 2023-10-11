using HagiDatabaseDomain;
using System;
using System.Text.Json;

namespace HagiDomain
{
    public class UserHttpClient : IDisposable
    {
        private readonly string _baseUrl;
        private HttpClient _httpClient;

        public UserHttpClient(HttpClient httpClient)
        {
            _baseUrl = "https://localhost:7066/User/";
            _httpClient = httpClient;
        }

        ~UserHttpClient()
        {
            Dispose();
        }

        public async Task<List<User>> GetUsers()
        {
            var httpResponseMessage = await _httpClient.GetAsync(_baseUrl);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<User>>(response);
            }

            throw new Exception();
        }


        public async Task<User> GetUserWithId(int id)
        {
            var url = _baseUrl + id;
            var httpResponseMessage = await _httpClient.GetAsync(url);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<User>(response);
            }

            throw new Exception();
        }

        //public async Task<User> CreateUser(string userName, string password)
        //{

        //}




        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}