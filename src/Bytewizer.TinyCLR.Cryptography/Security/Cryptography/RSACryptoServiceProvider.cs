namespace Bytewizer.TinyCLR.Security.Cryptography
{
    public sealed class RSACryptoServiceProvider : RSA
    {
        public RSACryptoServiceProvider()
            : this(1024)
        { }

        public RSACryptoServiceProvider(int dwKeySize)
        {
            KeySize = dwKeySize;
        }

        public override RSAParameters ExportParameters(bool includePrivateParameters)
        {
            throw new NotImplementedException();
        }

        public override void ImportParameters(RSAParameters parameters)
        {
            throw new NotImplementedException();
        }

        public override byte[] SignData(byte[] buffer, object halg)
        {
            throw new NotImplementedException();
        }

        public override byte[] SignHash(byte[] rgbHash, string? str)
        {
            throw new NotImplementedException();
        }

        public override bool VerifyData(byte[] buffer, object halg, byte[] signature)
        {
            throw new NotImplementedException();
        }

        public override bool VerifyHash(byte[] rgbHash, string str, byte[] rgbSignature)
        {
            throw new NotImplementedException();
        }
    }
}