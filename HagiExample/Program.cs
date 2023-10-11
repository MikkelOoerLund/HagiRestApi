




using HagiDatabaseDomain;
using HagiDomain;

class Program
{

    public static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        var userHttpClient = new UserHttpClient(httpClient);

        var userWithId = await userHttpClient.GetUserWithId(2);
        var users = await userHttpClient.GetUsers();

        Console.ReadKey();
    }
}