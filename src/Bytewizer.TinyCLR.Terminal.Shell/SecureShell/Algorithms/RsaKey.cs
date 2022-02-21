using System;

using Bytewizer.TinyCLR.Security.Cryptography;

namespace Bytewizer.TinyCLR.SecureShell.Algorithms
{
    public class RsaKey : PublicKeyAlgorithm
    {
        private readonly RSACryptoServiceProvider _algorithm = new RSACryptoServiceProvider();

        public RsaKey(RSAParameters parameters = new RSAParameters())
            : base(parameters)
        { }

        public override string Name
        {
            get { return "ssh-rsa"; }
        }

        public override void ImportKey(RSAParameters parameters)
        {
            _algorithm.ImportParameters(parameters);
        }

        public override RSAParameters ExportKey()
        {
            return _algorithm.ExportParameters(true);
        }

        public override void LoadKeyAndCertificatesData(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using (var worker = new SshDataStream(data))
            {
                if (worker.ReadString() != this.Name)
                    throw new Exception("Key and certificates were not created with this algorithm.");

                var args = new RSAParameters();
                args.Exponent = worker.ReadMpint();
                args.Modulus = worker.ReadMpint();

                _algorithm.ImportParameters(args);
            }
        }

        public override byte[] CreateKeyAndCertificatesData()
        {
            using (var worker = new SshDataStream())
            {
                var args = _algorithm.ExportParameters(false);

                worker.Write(this.Name);
                worker.WriteMpint(args.Exponent);
                worker.WriteMpint(args.Modulus);

                return worker.ToByteArray();
            }
        }

        public override bool VerifyData(byte[] data, byte[] signature)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (signature == null)
            {
                throw new ArgumentNullException(nameof(signature));
            }

            throw new NotImplementedException();
            //return _algorithm.VerifyData(data, "SHA1", signature);
        }

        public override bool VerifyHash(byte[] hash, byte[] signature)
        {
            if (hash == null)
            {
                throw new ArgumentNullException(nameof(hash));
            }

            if (signature == null)
            {
                throw new ArgumentNullException(nameof(signature));
            }

            throw new NotImplementedException();
            //return _algorithm.VerifyHash(hash, "SHA1", signature);
        }

        public override byte[] SignData(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            //throw new NotImplementedException();
            return _algorithm.SignData(data, "SHA1");
        }

        public override byte[] SignHash(byte[] hash)
        {
            if (hash == null)
            {
                throw new ArgumentNullException(nameof(hash));
            }

            throw new NotImplementedException();
            //return _algorithm.SignHash(hash, "1.3.14.3.2.29");
        }
    }
}