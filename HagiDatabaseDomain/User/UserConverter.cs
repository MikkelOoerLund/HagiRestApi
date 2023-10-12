namespace HagiDatabaseDomain
{
    public class UserConverter
    {
        public User ConvertUserLoginToUser(UserAuthenticationDTO userLogin)
        {
            return new User()
            {
                UserName = userLogin.UserName,
                HashPassword = userLogin.HashPassword,
            };
        }
    }
}