namespace HagiDatabaseDomain
{
    public class UserAuthenticationDTO
    {
        public int UserId { get; set; }
        public string Salt { get; set; }
        public string UserName { get; set; }
        public string HashPassword { get; set; }
    }
}