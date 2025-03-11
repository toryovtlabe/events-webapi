using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Business.Services
{
    public class HashService
    {
        private readonly int _iterationCount = 10000;
        private readonly int _saltSize = 128 / 8;
        private readonly int _numBytesRequested = 256 / 8;

        public string HashPassword(string password)
        {
            byte[] salt = new byte[_saltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: _iterationCount,
                numBytesRequested: _numBytesRequested));

            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(parts[0]);
            string hashed = parts[1];

            string hashToVerify = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: _iterationCount,
                numBytesRequested: _numBytesRequested));

            return hashed == hashToVerify;

        }
    }
}
