using MagicVillaApi.Helpers.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MagicVillaApi.Helpers
{
    public class PasswordService : IPasswordService
    {
        public PasswordHelperTO CreatePasswordHash(string password)
        {
             PasswordHelperTO obj = new();
            using (var hmac = new HMACSHA512())
            {
                obj.passwordSalt = hmac.Key;
                obj.passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
            return obj;
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
