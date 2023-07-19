using System.Security.Cryptography;
using ToDo.Services.Interfaces;

namespace ToDo.Services.Services
{
    /// <summary>
    /// Class implementing <see cref="IPasswordHasher"/>.
    /// </summary>
    public sealed class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10;

        /// <inheritdoc />
        public string HashPassword(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                       password,
                       SaltSize,
                       Iterations,
                       HashAlgorithmName.SHA512))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{salt}.{key}";
            }
        }

        /// <inheritdoc />
        public bool VerifyPassword(string hash, string password)
        {
            var parts = hash.Split(".".ToCharArray(), 2);

            if (parts.Length != 2)
            {
                throw new FormatException("Unexpected hash format. " +
                                          "Should be formatted as `{salt}.{hash}`");
            }

            var salt = Convert.FromBase64String(parts[0]);
            var key = Convert.FromBase64String(parts[1]);

            using (var algorithm = new Rfc2898DeriveBytes(
                       password,
                       salt,
                       Iterations,
                       HashAlgorithmName.SHA512))
            {
                var keyToCheck = algorithm.GetBytes(KeySize);

                var verified = keyToCheck.SequenceEqual(key);

                return verified;
            }
        }
    }
}
