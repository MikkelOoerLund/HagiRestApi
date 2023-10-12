namespace HagiDatabaseDomain
{
    public class User
    {
        public int UserId { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }

        public override string ToString()
        {
            return $"User ID: {UserId}, UserName: {Hash}, Password: {Salt}";
        }
    }
}