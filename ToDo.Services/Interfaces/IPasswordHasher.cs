using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Services.Interfaces
{
    /// <summary>
    /// Interface for password hasher and verifier.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hashes given password.
        /// </summary>
        /// <param name="password">Password to be hashed.</param>
        /// <returns>Generated hash.</returns>
        string HashPassword(string password);

        /// <summary>
        /// Verifies if given passwords matches given hash.
        /// </summary>
        /// <param name="hash">Hash to be verified.</param>
        /// <param name="password">Password to be verified.</param>
        /// <returns>True if password matches hash, false otherwise.</returns>
        bool VerifyPassword(string hash, string password);
    }
}
