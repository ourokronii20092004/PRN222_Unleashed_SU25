using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Utilities
{
    public class HashingPassword
    {
        // These constants define the parameters for PBKDF2
        private const int SaltSize = 16; // 16 bytes for the salt
        private const int HashSize = 32; // 32 bytes for the hash (SHA256 output size)
        private const int Iterations = 100000; // Increased iterations for better security, adjust as needed
        private static readonly HashAlgorithmName Pbkdf2HashAlgorithm = HashAlgorithmName.SHA256;

        
        public static string HashPassword(string password)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(password, nameof(password));
            // Generate a random salt
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            // Create the PBKDF2 hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, Pbkdf2HashAlgorithm);
            byte[] hash = pbkdf2.GetBytes(HashSize);


            // Combine the salt and hash
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convert to base64 for storage
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
           ArgumentNullException.ThrowIfNullOrEmpty(password, nameof (password));
           ArgumentNullException.ThrowIfNullOrEmpty(hashedPassword, nameof(hashedPassword));
            byte[] hashBytes;
            try
            {
                hashBytes = Convert.FromBase64String(hashedPassword);
            }
            catch (FormatException)
            {
                return false; // Stored hash is not valid Base64
            }


            // Check if the stored hash has the expected length
            if (hashBytes.Length != (SaltSize + HashSize))
            {
                return false; // Stored hash is corrupted or malformed
            }

            // Extract the salt from the stored hash
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize); // Use Buffer.BlockCopy for efficiency

            // Compute the hash of the provided password using the extracted salt and same parameters
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, Pbkdf2HashAlgorithm);
            byte[] computedHash = pbkdf2.GetBytes(HashSize);

            // Compare the computed hash with the stored hash using a constant-time comparison
            bool result = true;
            for (int i = 0; i < HashSize; i++)
            {
                // This logic ensures that the entire array is iterated,
                // preventing timing attacks by not short-circuiting on mismatch.
                if (hashBytes[i + SaltSize] != computedHash[i])
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
