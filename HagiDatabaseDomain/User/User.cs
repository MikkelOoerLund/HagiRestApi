namespace HagiDatabaseDomain
{

    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string Salt { get; set; }
        public string HashPassword { get; set; }


        public override string ToString()
        {
            return $"User ID: {UserId}, UserName: {UserName}, HashPassword: {HashPassword}, Salt: {Salt}";
        }
    }
}