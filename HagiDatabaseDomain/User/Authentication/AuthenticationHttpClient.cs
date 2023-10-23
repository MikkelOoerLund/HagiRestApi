using HagiDomain;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace HagiDatabaseDomain
{
    public class AuthenticationHttpClient : IDisposable
    {
        private readonly string _loginUrl;
        private readonly string _registerUrl;
        private UserAuthenticationFactory _userAuthenticationFactory;
        private HttpClient _httpClient;

        public AuthenticationHttpClient(HttpClient httpClient, UserAuthenticationFactory userAuthenticationFactory)
        {
            _httpClient = httpClient;
            _userAuthenticationFactory = userAuthenticationFactory;
            _loginUrl = "https://localhost:7066/Authentication/Login";
            _registerUrl = "https://localhost:7066/Authentication/Register";
        }

        ~AuthenticationHttpClient()
        {
            Dispose();
        }


        private async Task<UserAuthenticationDTO> GetUserAuthenticationAsync(string userName, string password)
        {
            var saltUrl = "https://localhost:7066/Authentication/" + userName;

            var httpSaltResponseMessage = await _httpClient.GetAsync(saltUrl);

            var salt = await ReadResponseAsync(httpSaltResponseMessage);

            var hashPassword = AuthenticationService.GenerateHashPassword(password, salt);

            return new UserAuthenticationDTO()
            {
                Salt = salt,
                UserName = userName,
                HashPassword = hashPassword,
            };
        }


        public async Task<string> RegisterAsync(string userName, string password)
        {

            var userAuthenticationDTO = _userAuthenticationFactory.CreateUserAuthentication(userName, password);

            var stringContent = CreateJsonStringObject(userAuthenticationDTO);
            var httpResponseMessage = await _httpClient.PostAsync(_registerUrl, stringContent);

            var jsonWebToken = await ReadResponseAsync(httpResponseMessage);
            return jsonWebToken;
        }

        public async Task<string> LoginAsync(string userName, string password)
        {

            var userAuthenticationDTO = await GetUserAuthenticationAsync(userName, password);


            var stringContent = CreateJsonStringObject(userAuthenticationDTO);
            var httpResponseMessage = await _httpClient.PostAsync(_loginUrl, stringContent);

            var jsonWebToken = await ReadResponseAsync(httpResponseMessage);
            return jsonWebToken;
        }


        private async Task<string> ReadResponseAsync(HttpResponseMessage httpResponseMessage)
        {
            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return response;
            }

            throw new Exception(response);
        }


        private async Task<T> DeserializeResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<T>(response);
            }

            throw new Exception(response);
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
