namespace HagiDatabaseDomain
{
    public class UserFactory
    {
        public User CreateUser(string userName, string password)
        {
            var salt = AuthenticationService.GenerateSalt();
            var hashPassword = AuthenticationService.GenerateHashPassword(password, salt);

            return new User()
            {
                Salt = salt,
                UserName = userName,
                HashPassword = hashPassword,
            };
        }
    }
}