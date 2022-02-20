namespace Bytewizer.TinyCLR.Security.Cryptography
{
    public abstract class RSA : AsymmetricAlgorithm
    {
        private static readonly KeySizes[] _legalKeySizes = { new KeySizes(384, 16384, 8) };

        protected RSA()
        {
            LegalKeySizesValue = _legalKeySizes;
        }

        public static new RSA Create()
        {
            return new RSACryptoServiceProvider();
        }
        public static RSA Create(int keySizeInBits)
        {
            RSA rsa = Create();

            try
            {
                rsa.KeySize = keySizeInBits;
                return rsa;
            }
            catch
            {
                rsa.Dispose();
                throw;
            }
        }
        public static RSA Create(RSAParameters parameters)
        {
            RSA rsa = Create();

            try
            {
                rsa.ImportParameters(parameters);
                return rsa;
            }
            catch
            {
                rsa.Dispose();
                throw;
            }
        }

        public abstract RSAParameters ExportParameters(bool includePrivateParameters);
        public abstract void ImportParameters(RSAParameters parameters);

        public abstract bool VerifyData(byte[] buffer, object halg, byte[] signature);
        public abstract bool VerifyHash(byte[] rgbHash, string str, byte[] rgbSignature);
        public abstract byte[] SignData(byte[] buffer, object halg);
        public abstract byte[] SignHash(byte[] rgbHash, string? str);
    }
}
