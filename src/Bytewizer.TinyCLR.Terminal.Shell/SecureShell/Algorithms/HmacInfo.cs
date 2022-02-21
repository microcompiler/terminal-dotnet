using System;

using Bytewizer.TinyCLR.Security.Cryptography;

namespace Bytewizer.TinyCLR.SecureShell.Algorithms
{
    public class HmacInfo
    {
        public HmacInfo(KeyedHashAlgorithm algorithm, int keySize)
        {
            if (algorithm == null)
            {
                throw new ArgumentNullException(nameof(algorithm));
            }

            KeySize = keySize;
            Hmac = key => new HmacAlgorithm(algorithm, keySize, key);
        }

        public int KeySize { get; private set; }

        public Func<byte[], HmacAlgorithm> Hmac { get; private set; }
    }
}
