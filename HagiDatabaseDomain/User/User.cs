namespace HagiDatabaseDomain
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

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