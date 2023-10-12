using HagiDatabaseDomain;
using System;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;

namespace HagiDomain
{
    public class UserHttpClient : IDisposable
    {
        private readonly string _baseUrl;

        private HttpClient _httpClient;

        public UserHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = "https://localhost:7066/User/";
        }

        ~UserHttpClient()
        {
            Dispose();
        }

        public async Task<List<User>> GetUsers()
        {
            var httpResponseMessage = await _httpClient.GetAsync(_baseUrl);
            return await DeserializeResponse<List<User>>(httpResponseMessage);
        }


        public async Task<User> GetUserWithId(int userId)
        {
            var url = _baseUrl + userId;
            var httpResponseMessage = await _httpClient.GetAsync(url);
            return await DeserializeResponse<User>(httpResponseMessage);
        }

        public async Task<User> CreateUserFromUserLogin(UserAuthenticationDTO userLogin)
        {
            var stringContent = CreateJsonStringObject(userLogin);
            var httpResponseMessage = await _httpClient.PostAsync(_baseUrl, stringContent);
            return await DeserializeResponse<User>(httpResponseMessage);
        }

   

        public async Task<User> UpdateUser(int userId, UserAuthenticationDTO userLogin)
        {
            var url = _baseUrl + userId;
            var stringContent = CreateJsonStringObject(userLogin);
            var httpResponseMessage = await _httpClient.PutAsync(url, stringContent);
            return await DeserializeResponse<User>(httpResponseMessage);
        }


        public async Task<bool> DeleteUser(int userId)
        {
            var url = _baseUrl + userId;
            var httpResponseMessage = await _httpClient.DeleteAsync(url);
            return httpResponseMessage.IsSuccessStatusCode;
        }



        private async Task<T> DeserializeResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(response);
            }

            throw new Exception("Failed to deserialize response.");
        }

        private StringContent CreateJsonStringObject<T>(T @object)
        {
            var json = JsonSerializer.Serialize(@object);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }


        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}