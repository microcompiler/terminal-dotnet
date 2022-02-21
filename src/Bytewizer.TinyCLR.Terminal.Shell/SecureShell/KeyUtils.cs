using System;

using Bytewizer.TinyCLR.Security.Cryptography;
using Bytewizer.TinyCLR.SecureShell.Algorithms;

namespace Bytewizer.TinyCLR.SecureShell
{
    public static class KeyUtils
    {
        public static string GetFingerprint(string sshkey)
        {
            if (sshkey == null)
            {
                throw new ArgumentNullException(nameof(sshkey));
            }

            using (var md5 = MD5.Create())
            {
                var bytes = Convert.FromBase64String(sshkey);
                bytes = md5.ComputeHash(bytes);
                return BitConverter.ToString(bytes).Replace("-", ":");
            }
        }

        private static PublicKeyAlgorithm GetKeyAlgorithm(string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            switch (type)
            {
                case "ssh-rsa":
                    return new RsaKey();
                //case "ssh-dss":
                //    return new DssKey();
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        public static RSAParameters GeneratePrivateKey(string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var alg = GetKeyAlgorithm(type);
            //var bytes = alg.ExportKey();
            //return Convert.ToBase64String(bytes);
            return alg.ExportKey();
        }

        public static string[] SupportedAlgorithms
        {
            get { return new string[] { "ssh-rsa", "ssh-dss" }; }
        }
    }
}
