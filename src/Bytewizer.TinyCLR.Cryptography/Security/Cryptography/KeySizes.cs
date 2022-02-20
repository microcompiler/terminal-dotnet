// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Bytewizer.TinyCLR.Security.Cryptography
{
    /// <summary>
    /// Determines the set of valid key sizes for the symmetric cryptographic algorithms.
    /// </summary>
    public sealed class KeySizes
    {
        /// <summary>
        /// Initializes a new instance of the KeySizes class with the specified key values.
        /// </summary>
        /// <param name="minSize">The minimum valid key size.</param>
        /// <param name="maxSize">The maximum valid key size.</param>
        /// <param name="skipSize">The interval between valid key sizes.</param>
        public KeySizes(int minSize, int maxSize, int skipSize)
        {
            MinSize = minSize;
            MaxSize = maxSize;
            SkipSize = skipSize;
        }

        /// <summary>
        /// Specifies the minimum key size in bits.
        /// </summary>
        public int MinSize { get; private set; }

        /// <summary>
        /// Specifies the maximum key size in bits.
        /// </summary>
        public int MaxSize { get; private set; }

        /// <summary>
        /// Specifies the interval between valid key sizes in bits.
        /// </summary>
        public int SkipSize { get; private set; }

        internal bool IsLegal(int keySize)
        {
            int ks = keySize - MinSize;
            bool result = (ks >= 0) && (keySize <= MaxSize);

            return (SkipSize == 0) ? result : (result && (ks % SkipSize == 0));
        }

        internal static bool IsLegalKeySize(KeySizes[] legalKeys, int size)
        {
            foreach (KeySizes legalKeySize in legalKeys)
            {
                if (legalKeySize.IsLegal(size))
                {
                    return true;
                }
            }

            return false;
        }
    }
}