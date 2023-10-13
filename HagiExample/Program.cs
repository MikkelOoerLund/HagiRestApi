﻿




using HagiDatabaseDomain;
using HagiDomain;
using System.Collections.Generic;

class Program
{

    public static async Task Main()
    {

        await UserAuthorizationExampleAsync();
        Console.ReadLine();
    }

    //public static void Main(string[] args)
    //{
    //    var password = "hegne";
    //    var salt = AuthenticationService.GenerateSalt();
    //    var hash = AuthenticationService.GenerateHashPassword(password, salt);
    //    var anotherHash = AuthenticationService.GenerateHashPassword(password, salt);

    //    Console.WriteLine(hash);
    //    Console.WriteLine(anotherHash);


    //    Console.ReadLine();
    //}

    private void BCryptExample()
    {
        var password = "Hegne";
        var saltCount = 20;
        var hashIterationFactor = 14;


        var salt = BCrypto.GenerateSalt(saltCount);
        var saltWithPassword = salt + password;


        string passwordHash = BCrypto.HashPassword(saltWithPassword, hashIterationFactor);


        var isValid = BCrypto.Verify(saltWithPassword, passwordHash);


        Console.WriteLine(salt);
        Console.WriteLine(passwordHash);

        Console.WriteLine(isValid);


        Console.ReadKey();
    }


    private static async Task UserAuthorizationExampleAsync()
    {
        var httpClient = new HttpClient();
        var userHttpClient = new UserHttpClient(httpClient);
        var userAuthenticationHttpClient = new UserAuthenticationHttpClient(httpClient);

        var userAuthentication = new UserAuthenticationDTO()
        {
            Salt = "random_salt_value",
            UserName = "random_user_name",
            HashPassword = "random_hash_password"
}       ;


        var user = await userHttpClient.CreateUserFromAuthenticationAsync(userAuthentication);
        var jsonWebToken = await userAuthenticationHttpClient.LoginUserAsync(userAuthentication);

        var value = await userAuthenticationHttpClient.GetAuthorizedExampleData(jsonWebToken);

        await Console.Out.WriteLineAsync(value);
    }

    private static async Task UserHttpClientExampleAsync()
    {

        // Initialize example
        var httpClient = new HttpClient();
        var userHttpClient = new UserHttpClient(httpClient);

        // Create user example

        var dateTime = DateTime.Now;
        var dateTimeString = dateTime.ToString();

        var factory = new UserAuthenticationFactory();


        var userName = "Hegne";
        var password = "Firkant";

        var userAuthentication = factory.CreateUserAuthentication(userName, password);

        var createdUser = await userHttpClient.CreateUserFromAuthenticationAsync(userAuthentication);
        await Console.Out.WriteLineAsync($"Created user:");
        await Console.Out.WriteLineAsync($" - {createdUser}");
        await Console.Out.WriteLineAsync();

        // Get user with name example

        var userWithName = await userHttpClient.GetUserWithNameAsync(createdUser.UserName);

        await Console.Out.WriteLineAsync($"Got user with name:");
        await Console.Out.WriteLineAsync($" - {userWithName}");
        await Console.Out.WriteLineAsync();


        // Get user with id example
        var userWithId = await userHttpClient.GetUserWithIdAsync(createdUser.UserId);

        await Console.Out.WriteLineAsync($"Got user with id:");
        await Console.Out.WriteLineAsync($" - {userWithId}");
        await Console.Out.WriteLineAsync();


        // Get all users example
        var users = await userHttpClient.GetUsersAsync();

        await Console.Out.WriteLineAsync("All users:");

        foreach (var user in users)
        {
            await Console.Out.WriteLineAsync($" - {user}");
        }

        if (users.Count == 0)
        {
            await Console.Out.WriteLineAsync("  - No users found");
        }


        await Console.Out.WriteLineAsync();





        // Update user example
        var updatedUser = await userHttpClient.UpdateUser(createdUser.UserId, userAuthentication);


        await Console.Out.WriteLineAsync($"Updated user: ");
        await Console.Out.WriteLineAsync($" - {updatedUser}");
        await Console.Out.WriteLineAsync();


        // Delete user example
        var hasDeletedUser = await userHttpClient.DeleteUser(createdUser.UserId);


        await Console.Out.WriteLineAsync($"Try to delete user with id:");
        await Console.Out.WriteLineAsync($" - Has deleted user with id {createdUser.UserId}: {hasDeletedUser}");
        await Console.Out.WriteLineAsync();


        // View updated collection
        await Console.Out.WriteLineAsync("All users:");

        var updatedUsers = await userHttpClient.GetUsersAsync();

        foreach (var user in updatedUsers)
        {
            await Console.Out.WriteLineAsync($" - {user}");
        }

        if (updatedUsers.Count == 0)
        {
            await Console.Out.WriteLineAsync(" - No users found");
        }
    }
}