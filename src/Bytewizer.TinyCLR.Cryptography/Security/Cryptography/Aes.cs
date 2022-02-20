// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Bytewizer.TinyCLR.Security.Cryptography
{
    public abstract class AES : SymmetricAlgorithm
    {
        private static readonly KeySizes[] _legalBlockSizes = { new KeySizes(128, 128, 0) };
        private static readonly KeySizes[] _legalKeySizes = { new KeySizes(128, 256, 64) };

        protected AES()
        {
            LegalBlockSizesValue = _legalBlockSizes;
            LegalKeySizesValue = _legalKeySizes;

            BlockSizeValue = 128;
            FeedbackSizeValue = 8;
            KeySizeValue = 256;
            ModeValue = CipherMode.CBC;
        }

        public static new AES Create()
        {
            return new AESCryptoServiceProvider();
        }
    }
}