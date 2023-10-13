using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace HagiDatabaseDomain
{
    // Note: Refactor
    public class UserAuthenticationHttpClient : IDisposable
    {
        private readonly string _registerUrl;
        private readonly string _loginUrl;
        private HttpClient _httpClient;


        public UserAuthenticationHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _registerUrl = "https://localhost:7066/Authentication/Register";
            _loginUrl = "https://localhost:7066/Authentication/Login";
        }

        ~UserAuthenticationHttpClient()
        {
            Dispose();
        }

        // Remove or create new example class
        public async Task<string> GetAuthorizedExampleData(string jsonWebToken)
        {
            var exampleUrl = "https://localhost:7066/ExampleAuthorization";
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, exampleUrl);
            var headers = httpRequestMessage.Headers;
            headers.Add("Authorization", $"Bearer {jsonWebToken}");


            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            var responseContent = httpResponseMessage.Content;
            var response = await responseContent.ReadAsStringAsync();

            return response;
        }

        public async Task<string> RegisterUserAsync(UserAuthenticationDTO userAuthentication)
        {
            var userAuthenticationJson = CreateJsonStringObject(userAuthentication);
            var httpResponseMessage = await _httpClient.PostAsync(_registerUrl, userAuthenticationJson);
            var responseContent = httpResponseMessage.Content;
            var response = await responseContent.ReadAsStringAsync();

            return response;
        }

        public async Task<string> LoginUserAsync(UserAuthenticationDTO userAuthentication)
        {
            var userAuthenticationJson = CreateJsonStringObject(userAuthentication);
            var httpResponseMessage = await _httpClient.PostAsync(_loginUrl, userAuthenticationJson);
            var responseContent = httpResponseMessage.Content;
            var response = await responseContent.ReadAsStringAsync();

            return response;
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
