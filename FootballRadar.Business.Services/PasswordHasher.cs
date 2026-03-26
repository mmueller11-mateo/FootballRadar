using FootballRadar.Abstractions;
using System.Security.Cryptography;

namespace FootballRadar.Business.Services
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
            return $"{Convert.ToHexString(hash)}.{Convert.ToHexString(salt)}";
        }

        public bool Verify(string password, string storedHash)
        {
            var parts = storedHash.Split('.');
            var hash = Convert.FromHexString(parts[0]);
            var salt = Convert.FromHexString(parts[1]);
            var newHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
            return CryptographicOperations.FixedTimeEquals(hash, newHash);
        }
    }
}