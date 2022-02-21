using System;

namespace Bytewizer.TinyCLR.SecureShell.Algorithms
{
    public class NoCompression : CompressionAlgorithm
    {
        public override byte[] Compress(byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return input;
        }

        public override byte[] Decompress(byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return input;
        }
    }
}