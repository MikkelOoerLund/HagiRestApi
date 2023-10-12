namespace HagiDatabaseDomain
{
    public class UserConverter
    {
        public User ConvertUserLoginToUser(UserAuthenticationDTO userAuthentication)
        {
            return new User()
            {
                Salt = userAuthentication.Salt,
                UserName = userAuthentication.UserName,
                HashPassword = userAuthentication.HashPassword,
            };
        }
    }
}