




using HagiDatabaseDomain;
using HagiDomain;

class Program
{

    public static void Main(string[] args)
    {
        var httpClient = new HttpClient();
        var userHttpClient = new UserHttpClient(httpClient);

        var userWithId = userHttpClient.GetUserWithId(3);


    }
}