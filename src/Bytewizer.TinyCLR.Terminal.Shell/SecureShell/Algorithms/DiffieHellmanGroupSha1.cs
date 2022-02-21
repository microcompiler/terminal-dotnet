using System;

using Bytewizer.TinyCLR.Security.Cryptography;

namespace Bytewizer.TinyCLR.SecureShell.Algorithms
{
    public class DiffieHellmanGroupSha1 : KexAlgorithm
    {
        private readonly DiffieHellman _exchangeAlgorithm;

        public DiffieHellmanGroupSha1(DiffieHellman algorithm)
        {
            //Contract.Requires(algorithm != null);

            _exchangeAlgorithm = algorithm;
            _hashAlgorithm = new SHA1CryptoServiceProvider();
        }

        public override byte[] CreateKeyExchange()
        {
            return _exchangeAlgorithm.CreateKeyExchange();
        }

        public override byte[] DecryptKeyExchange(byte[] exchangeData)
        {
            if (exchangeData == null)
            {
                throw new ArgumentNullException(nameof(exchangeData));
            }

            return _exchangeAlgorithm.DecryptKeyExchange(exchangeData);
        }
    }
}
