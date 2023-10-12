using System.Security.Cryptography;

namespace HagiDatabaseDomain
{
    //public static class Assert
    //{
    //    public static void AssertValueInRange(int value, int minValue, int maxValue)
    //    {
    //        if (value < minValue) throw new Exception($"Value: {value} can not be less then MinValue: {minValue}");
    //        if (value > maxValue) throw new Exception($"Value: {value} can not be less then MinValue: {minValue}");
    //    }

    //}

    public static class AuthenticationService
    {
        private const int SALT_SIZE = 32;
        private const int HASH_SIZE = 32;
        private const int HASH_ITERATION_COUNT = 600000;

        public static string GenerateSalt()
        {
            var randomNumberGenerator = RandomNumberGenerator.Create();
            var saltBytes = new byte[SALT_SIZE];

            randomNumberGenerator.GetBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }


        public static string GenerateHashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using var hashFactory = new Rfc2898DeriveBytes(password, saltBytes, HASH_ITERATION_COUNT);
            var hashBytes = hashFactory.GetBytes(HASH_SIZE);
            return Convert.ToBase64String(hashBytes);
        }



    }
}