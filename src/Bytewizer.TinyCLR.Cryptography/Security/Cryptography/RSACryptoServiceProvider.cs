namespace Bytewizer.TinyCLR.Security.Cryptography
{
    public sealed class RSACryptoServiceProvider : RSA
    {
        private readonly System.Security.Cryptography.RSA provider;
        
        public RSACryptoServiceProvider()
            : this(1024)
        { }

        public RSACryptoServiceProvider(int dwKeySize)
        {
            provider = System.Security.Cryptography.RSA.Create();
            KeySize = dwKeySize;
        }

        public override RSAParameters ExportParameters(bool includePrivateParameters)
        {
            var parm = provider.ExportParameters(includePrivateParameters);
            return new RSAParameters()
            {
                D = parm.D,
                DP = parm.DP,
                DQ = parm.DQ,
                Exponent = parm.Exponent,
                InverseQ = parm.InverseQ,
                Modulus = parm.Modulus,
                P = parm.P,
                Q = parm.Q
            };
        }

        public override void ImportParameters(RSAParameters parameters)
        {
            var parm = new System.Security.Cryptography.RSAParameters()
            {
                D = parameters.D,
                DP = parameters.DP,
                DQ = parameters.DQ,
                Exponent = parameters.Exponent,
                InverseQ = parameters.InverseQ,
                Modulus = parameters.Modulus,
                P = parameters.P,
                Q = parameters.Q
            };

            provider.ImportParameters(parm);
        }

        public override byte[] SignData(byte[] buffer, object halg)
        {
            return provider.SignData(
                    buffer,
                    System.Security.Cryptography.HashAlgorithmName.SHA1, 
                    System.Security.Cryptography.RSASignaturePadding.Pkcs1 
                );
        }

        public override byte[] SignHash(byte[] rgbHash, string? str)
        {
            return provider.SignHash(
                    rgbHash, 
                    System.Security.Cryptography.HashAlgorithmName.SHA1, 
                    System.Security.Cryptography.RSASignaturePadding.Pkcs1
                ); 
        }

        public override bool VerifyData(byte[] buffer, object halg, byte[] signature)
        {
            return provider.VerifyData(
                    buffer, 
                    signature, 
                    System.Security.Cryptography.HashAlgorithmName.SHA1,
                    System.Security.Cryptography.RSASignaturePadding.Pkcs1
                );
        }

        public override bool VerifyHash(byte[] rgbHash, string str, byte[] rgbSignature)
        {
            return provider.VerifyHash(
                    rgbHash,
                    rgbSignature,
                    System.Security.Cryptography.HashAlgorithmName.SHA1,
                    System.Security.Cryptography.RSASignaturePadding.Pkcs1
                );
        }
    }
}