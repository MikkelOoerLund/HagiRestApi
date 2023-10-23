using HagiDatabaseDomain;
using System.Collections.Generic;


class Program
{

    public static async Task Main()
    {
        //var userHttpExample = new UserHttpExample();

        //await userHttpExample.RunAsync();

        var userAuthenticationFactory = new UserAuthenticationFactory();


        var userName = "Ost";
        var password = "Firkant";

        var httpClient = new HttpClient();
        var authenticationHttpClient = new AuthenticationHttpClient(httpClient, userAuthenticationFactory);
        await authenticationHttpClient.RegisterAsync(userName, password);


        //await authenticationHttpClient.LoginAsync(userName, password);



        Console.ReadLine();
    }
}