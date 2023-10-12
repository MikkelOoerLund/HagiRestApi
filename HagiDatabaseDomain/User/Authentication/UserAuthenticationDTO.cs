namespace HagiDatabaseDomain
{
    public class UserAuthenticationDTO
    {
        public string Salt { get; set; }
        public string UserName { get; set; }
        public string HashPassword { get; set; }
    }
}