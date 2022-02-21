using Bytewizer.TinyCLR.Security.Cryptography;

namespace Bytewizer.TinyCLR.SecureShell.Algorithms
{
    public class HmacAlgorithm
    {
        private readonly KeyedHashAlgorithm _algorithm;

        public HmacAlgorithm(KeyedHashAlgorithm algorithm, int keySize, byte[] key)
        {
            //Contract.Requires(algorithm != null);
            //Contract.Requires(key != null);
            //Contract.Requires(keySize == key.Length << 3);

            _algorithm = algorithm;
            algorithm.Key = key;
        }

        public int DigestLength
        {
            get { return _algorithm.HashSize >> 3; }
        }

        public byte[] ComputeHash(byte[] input)
        {
            //Contract.Requires(input != null);

            return _algorithm.ComputeHash(input);
        }
    }
}
