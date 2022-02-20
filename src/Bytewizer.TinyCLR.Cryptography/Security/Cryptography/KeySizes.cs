// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Bytewizer.TinyCLR.Security.Cryptography
{
    public sealed class KeySizes
    {
        public KeySizes(int minSize, int maxSize, int skipSize)
        {
            MinSize = minSize;
            MaxSize = maxSize;
            SkipSize = skipSize;
        }

        public int MinSize { get; private set; }
        public int MaxSize { get; private set; }
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