namespace HagiDatabaseDomain
{
    public class UserAuthenticationDTO
    {
        public string UserName { get; set; }
        public string HashPassword { get; set; }
        public string Salt { get; set; }
    }
}