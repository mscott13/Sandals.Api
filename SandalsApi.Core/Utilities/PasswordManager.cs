using SandalsApi.Core.Data;
using SandalsApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SandalsApi.Core.Utilities
{
    public class PasswordManager
    {
        public static string GetHash(string pswd)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[20];
            rng.GetBytes(salt);

            var rfc2898 = new Rfc2898DeriveBytes(pswd, salt, 10000);
            byte[] hash = rfc2898.GetBytes(20);

            byte[] combine = new byte[40];
            Array.Copy(salt, 0, combine, 0, 20);
            Array.Copy(hash, 0, combine, 20, 20);

            var result = Convert.ToBase64String(combine);
            return result;
        }

        public static bool Authenticate(string[] credentials)
        {
            User user = Database.GetUser(credentials[0], true);
            if (user != null)
            {
                return VerifyCredentials(credentials[1], user.hash);
            }
            return false;
        }

        public static bool VerifyCredentials(string pswd, string dbhash)
        {
            try
            {
                byte[] combine = Convert.FromBase64String(dbhash);
                byte[] salt = new byte[20];

                Array.Copy(combine, 0, salt, 0, 20);
                var rfc2898 = new Rfc2898DeriveBytes(pswd, salt, 10000);
                byte[] pwhash = rfc2898.GetBytes(20);

                for (int i = 0; i < 20; i++)
                {
                    if (combine[i + 20] != pwhash[i])
                    {
                        throw new UnauthorizedAccessException();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ResetPassword(int userId)
        {
            Database.SetAccountRecoveryState(userId, true);
            Database.SetRecoveryCode(userId, GenerateRecoveryCode());
        }

        public static bool ChangePassword(string username, string old_psw, string new_psw)
        {
            User user = Database.GetUser(username, true);
            if (user != null)
            {
                if (VerifyCredentials(old_psw, user.hash))
                {
                    return Database.UpdatePassword(username, GetHash(new_psw)); ;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public static string GenerateRecoveryCode()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] guid = Guid.NewGuid().ToByteArray();
            string key = Convert.ToBase64String(time.Concat(guid).ToArray());
            return key.Substring(0, 6);
        }

        public static bool CheckPasswordRequirement(string input) 
        {
            string patterm = @"^(?=.{6,})(?=.*[~*!@#$%^&+=]).*$";
            return Regex.IsMatch(input, patterm);
        }
    }
}
