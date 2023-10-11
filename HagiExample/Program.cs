




using HagiDatabaseDomain;

class Program
{

    public static void Main(string[] args)
    {

        using (var userContext = new UserContext())
        {
            var database = userContext.Database;
            database.EnsureCreated();
        }
    }
}