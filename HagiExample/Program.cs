




using HagiDatabaseDomain;
using HagiDomain;

class Program
{

    public static async Task Main(string[] args)
    {

        // Initialize example
        var httpClient = new HttpClient();
        var userHttpClient = new UserHttpClient(httpClient);

        // Create user example

        var dateTime = DateTime.Now;
        var userLogin = new UserLoginDTO()
        {
            UserName = "Hegne: " + dateTime.ToString(),
            Password = "Firkant: " + dateTime.ToString(),
        };


        var createdUser = await userHttpClient.CreateUserFromUserLogin(userLogin);
        await Console.Out.WriteLineAsync($"Created user: {createdUser}");
        await Console.Out.WriteLineAsync($" - {createdUser}");
        await Console.Out.WriteLineAsync();


        // Get user with id example
        var userWithId = await userHttpClient.GetUserWithId(createdUser.UserId);

        await Console.Out.WriteLineAsync($"Got user with id:");
        await Console.Out.WriteLineAsync($" - {userWithId}");
        await Console.Out.WriteLineAsync();


        // Get all users example
        var users = await userHttpClient.GetUsers();

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
        var updatedUser = await userHttpClient.UpdateUser(createdUser.UserId, userLogin);


        await Console.Out.WriteLineAsync($"Updated user: {updatedUser}");
        await Console.Out.WriteLineAsync($" - {updatedUser}");
        await Console.Out.WriteLineAsync();


        // Delete user example
        var hasDeletedUser = await userHttpClient.DeleteUser(createdUser.UserId);


        await Console.Out.WriteLineAsync($"Try to delete user with id:");
        await Console.Out.WriteLineAsync($" - Has deleted user with id {createdUser.UserId}: {hasDeletedUser}");
        await Console.Out.WriteLineAsync();


        // View updated collection
        await Console.Out.WriteLineAsync("All users:");

        var updatedUsers = await userHttpClient.GetUsers();

        foreach (var user in updatedUsers)
        {
            await Console.Out.WriteLineAsync($" - {user}");
        }

        if (updatedUsers.Count == 0)
        {
            await Console.Out.WriteLineAsync(" - No users found");
        }

        Console.ReadKey();
    }
}