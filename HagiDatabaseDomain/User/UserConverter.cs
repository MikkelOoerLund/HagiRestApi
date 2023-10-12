namespace HagiDatabaseDomain
{
    public class UserConverter
    {
        public User ConvertUserLoginToUser(UserLoginDTO userLogin)
        {
            return new User()
            {
                Hash = userLogin.UserName,
                Salt = userLogin.Password,
            };
        }
    }
}