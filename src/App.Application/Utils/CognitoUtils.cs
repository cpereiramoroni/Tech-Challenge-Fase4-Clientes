using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Utils
{
    public static class CognitoUtils
    {
        public static string CalculateSecretHash(string clientId, string clientSecret, string username)
        {
            var data = $"{username}{clientId}";
            var key = Encoding.UTF8.GetBytes(clientSecret);

            using var hmac = new HMACSHA256(key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hash);
        }
    }

}
