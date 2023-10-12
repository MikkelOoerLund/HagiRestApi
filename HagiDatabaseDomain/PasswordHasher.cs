using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HagiDatabaseDomain
{
    public static class CryptographyService
    {

        public static byte[] GenerateRandomSaltInBytes(int length)
        {
            var saltBytes = new byte[length];

            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(saltBytes);

            return saltBytes;
        }
    }

    public class PasswordHasher
    {

        public (string Salt, string Hash) HashPassword(string password, int saltSize, int hashSize, int numberOfHashIterations)
        {
            byte[] saltBytes = new byte[saltSize];

            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(saltBytes);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltSize, numberOfHashIterations))
            {
                byte[] hashBytes = pbkdf2.GetBytes(hashSize);

                var saltString = Convert.ToBase64String(saltBytes);
                var hashString = Convert.ToBase64String(hashBytes);

                return (saltString, hashString);
            }
        }

        public bool VerifyPassword(string password, string salt, string hash, int hashSize, int numberOfHashIterations)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] hashBytes = Convert.FromBase64String(hash);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, numberOfHashIterations))
            {
                byte[] testHash = pbkdf2.GetBytes(hashSize);

                for (int i = 0; i < hashSize; i++)
                {
                    if (hashBytes[i] != testHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }



}
