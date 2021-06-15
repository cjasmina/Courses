using System;
using System.Text;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Courses.Helpers
{
    public static class Kriptografija
    {
        public static string Hashiraj(string lozinka, string salt)
        {
            var bajtovi = KeyDerivation.Pbkdf2(
                lozinka, 
                Encoding.UTF8.GetBytes(salt), 
                KeyDerivationPrf.HMACSHA512,
                10000, 
                32
            );

            return Convert.ToBase64String(bajtovi);
        }

        public static bool Validiraj(string lozinka, string salt, string hash)
        {
            return Hashiraj(lozinka, salt) == hash;
        }
    }
}
