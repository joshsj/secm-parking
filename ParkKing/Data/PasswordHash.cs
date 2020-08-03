using System.Security.Cryptography;

namespace ParkKing.Data
{
    public sealed class PasswordHash
    {
        // as advised by https://crackstation.net/hashing-security.htm

        // Size of salt/hash
        private static int Size => 32;

        // Amount of hash iterations to perform
        private static int HashIterations => 10000;

        private byte[] Hash { get; set; }
        private byte[] Salt { get; set; }

        public PasswordHash(string password)
        {
            Salt = new byte[Size];
            new RNGCryptoServiceProvider().GetBytes(Salt);

            Hash = new Rfc2898DeriveBytes(password, Salt, HashIterations).GetBytes(Size);
        }

        public bool Verify(string password)
        {
            // generate hash of inputted password
            byte[] testHash = new Rfc2898DeriveBytes(password, Salt, HashIterations).GetBytes(Size);
            
            // check all bytes
            for (int i = 0; i < Size; i++)
            {
                if (testHash[i] != Hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
