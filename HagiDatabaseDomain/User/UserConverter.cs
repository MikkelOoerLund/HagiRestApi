namespace HagiDatabaseDomain
{
    public class UserConverter
    {
        public User ConvertUserLoginToUser(UserLoginDTO userLogin)
        {
            return new User()
            {
                UserName = userLogin.UserName,
                Password = userLogin.Password,
            };
        }
    }
}