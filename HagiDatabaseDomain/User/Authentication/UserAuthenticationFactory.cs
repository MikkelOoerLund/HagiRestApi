namespace HagiDatabaseDomain
{
    public class UserAuthenticationFactory
    {
        public UserAuthenticationDTO CreateUserAuthentication(string userName, string password)
        {
            var salt = AuthenticationService.GenerateSalt();
            var hashPassword = AuthenticationService.GenerateHashPassword(password, salt);

            return new UserAuthenticationDTO()
            {
                Salt = salt,
                UserName = userName,
                HashPassword = hashPassword,
            };
        }
    }
}