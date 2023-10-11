using HagiDatabaseDomain;
using System.Text.Json;

namespace HagiDomain
{
    //public interface ISerializer
    //{
    //    public string SerializeObject<T>(T @object);
    //    public T DeserializeObject<T>(T @object);
    //}

    public class UserHttpClient : IDisposable
    {
        private readonly string _baseUrl;
        private HttpClient _httpClient;

        public UserHttpClient(HttpClient httpClient)
        {
            _baseUrl = "https://localhost:7066/User";
            _httpClient = httpClient;
        }

        ~UserHttpClient()
        {
            Dispose();
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


        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}