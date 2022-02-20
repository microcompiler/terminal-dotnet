using System;
using System.Text;
using System.Diagnostics.Contracts;

using Bytewizer.TinyCLR.Security.Cryptography;

namespace FxSsh.Algorithms
{
    public abstract class PublicKeyAlgorithm
    {
        public PublicKeyAlgorithm(RSAParameters parameters)
        {
            ImportKey(parameters);
        }

        public abstract string Name { get; }

        public string GetFingerprint()
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(CreateKeyAndCertificatesData());
                return BitConverter.ToString(bytes).Replace('-', ':');
            }
        }

        public byte[] GetSignature(byte[] signatureData)
        {
            Contract.Requires(signatureData != null);

            using (var worker = new SshDataWorker(signatureData))
            {
                if (worker.ReadString(Encoding.ASCII) != this.Name)
                    throw new Exception("Signature was not created with this algorithm.");

                var signature = worker.ReadBinary();
                return signature;
            }
        }

        public byte[] CreateSignatureData(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using (var worker = new SshDataWorker())
            {
                var signature = SignData(data);

                worker.Write(this.Name, Encoding.ASCII);
                worker.WriteBinary(signature);

                return worker.ToByteArray();
            }
        }

        public abstract void ImportKey(RSAParameters parameters);

        public abstract RSAParameters ExportKey();

        public abstract void LoadKeyAndCertificatesData(byte[] data);

        public abstract byte[] CreateKeyAndCertificatesData();

        public abstract bool VerifyData(byte[] data, byte[] signature);

        public abstract bool VerifyHash(byte[] hash, byte[] signature);

        public abstract byte[] SignData(byte[] data);

        public abstract byte[] SignHash(byte[] hash);
    }
}
