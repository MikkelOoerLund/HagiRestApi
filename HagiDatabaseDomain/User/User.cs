namespace HagiDatabaseDomain
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return $"User ID: {UserId}, UserName: {UserName}, Password: {Password}";
        }
    }
}