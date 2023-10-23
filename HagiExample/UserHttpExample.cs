




using HagiDatabaseDomain;
using HagiDomain;

class UserHttpExample
{

    private UserHttpClient _userHttpClient;


    public UserHttpExample()
    {
        var httpClient = new HttpClient();
        _userHttpClient = new UserHttpClient(httpClient);
    }

    public async Task RunAsync()
    {
        // Create user example

        var dateTime = DateTime.Now;
        var dateTimeString = dateTime.ToString();

        var factory = new UserAuthenticationFactory();


        var userName = "Hegne";
        var password = "Firkant";

        var userAuthentication = factory.CreateUserAuthentication(userName, password);

        var createdUser = await _userHttpClient.CreateUserFromAuthenticationAsync(userAuthentication);
        await Console.Out.WriteLineAsync($"Created user:");
        await Console.Out.WriteLineAsync($" - {createdUser}");
        await Console.Out.WriteLineAsync();

        // Get user with name example

        var userWithName = await _userHttpClient.GetUserWithNameAsync(createdUser.UserName);

        await Console.Out.WriteLineAsync($"Got user with name:");
        await Console.Out.WriteLineAsync($" - {userWithName}");
        await Console.Out.WriteLineAsync();


        // Get user with id example
        var userWithId = await _userHttpClient.GetUserWithIdAsync(createdUser.UserId);

        await Console.Out.WriteLineAsync($"Got user with id:");
        await Console.Out.WriteLineAsync($" - {userWithId}");
        await Console.Out.WriteLineAsync();


        // Get all users example
        var users = await _userHttpClient.GetUsersAsync();

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
        var updatedUser = await _userHttpClient.UpdateUser(createdUser.UserId, userAuthentication);


        await Console.Out.WriteLineAsync($"Updated user: ");
        await Console.Out.WriteLineAsync($" - {updatedUser}");
        await Console.Out.WriteLineAsync();


        // Delete user example
        var hasDeletedUser = await _userHttpClient.DeleteUser(createdUser.UserId);


        await Console.Out.WriteLineAsync($"Try to delete user with id:");
        await Console.Out.WriteLineAsync($" - Has deleted user with id {createdUser.UserId}: {hasDeletedUser}");
        await Console.Out.WriteLineAsync();


        // View updated collection
        await Console.Out.WriteLineAsync("All users:");

        var updatedUsers = await _userHttpClient.GetUsersAsync();

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
