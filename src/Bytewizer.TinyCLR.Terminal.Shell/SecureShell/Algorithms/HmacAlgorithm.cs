using System;

using Bytewizer.TinyCLR.Security.Cryptography;

namespace Bytewizer.TinyCLR.SecureShell.Algorithms
{
    public class HmacAlgorithm
    {
        private readonly KeyedHashAlgorithm _algorithm;

        public HmacAlgorithm(KeyedHashAlgorithm algorithm, int keySize, byte[] key)
        {
            if (algorithm == null)
            {
                throw new ArgumentNullException(nameof(algorithm));
            }

            if (keySize != key.Length << 3)
            {
                throw new ArgumentNullException(nameof(key));
            }

            _algorithm = algorithm;
            algorithm.Key = key;
        }

        public int DigestLength
        {
            get { return _algorithm.HashSize >> 3; }
        }

        public byte[] ComputeHash(byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return _algorithm.ComputeHash(input);
        }
    }
}
