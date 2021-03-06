using System;
using Bytewizer.TinyCLR.Security.Cryptography;

namespace Bytewizer.TinyCLR.SecureShell.Algorithms
{
    public class EncryptionAlgorithm
    {
        private readonly SymmetricAlgorithm _algorithm;
        private readonly CipherModeEx _mode;
        private readonly ICryptoTransform _transform;

        public EncryptionAlgorithm(SymmetricAlgorithm algorithm, int keySize, CipherModeEx mode, byte[] key, byte[] iv, bool isEncryption)
        {

            if (algorithm == null)
            {
                throw new ArgumentNullException(nameof(algorithm));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (iv == null)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            if (keySize != key.Length << 3)
            {
                throw new ArgumentNullException(nameof(keySize));
            }

            algorithm.KeySize = keySize;
            algorithm.Key = key;
            algorithm.IV = iv;
            algorithm.Padding = PaddingMode.None;

            _algorithm = algorithm;
            _mode = mode;

            _transform = CreateTransform(isEncryption);
        }

        public int BlockBytesSize
        {
            get { return _algorithm.BlockSize >> 3; }
        }

        public byte[] Transform(byte[] input)
        {
            var output = new byte[input.Length];
            _transform.TransformBlock(input, 0, input.Length, output, 0);
            return output;
        }

        private ICryptoTransform CreateTransform(bool isEncryption)
        {
            switch (_mode)
            {
                case CipherModeEx.CBC:
                    _algorithm.Mode = CipherMode.CBC;
                    return isEncryption
                        ? _algorithm.CreateEncryptor()
                        : _algorithm.CreateDecryptor();
                case CipherModeEx.CTR:
                    return new CtrModeCryptoTransform(_algorithm);
                default:
                    throw new ArgumentException(string.Format("Invalid mode: {0}", _mode));
            }
        }
    }
}