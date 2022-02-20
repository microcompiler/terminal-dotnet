// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Bytewizer.TinyCLR.Security.Cryptography
{
    public class HMACSHA1 : HMAC 
    {
        public HMACSHA1 () 
            : this (KeyBuilder.Key(64)) 
        {}

        public HMACSHA1 (byte[] key) {
            m_hashName = "SHA1";
            m_hash1 = new SHA1CryptoServiceProvider();
            m_hash2 = new SHA1CryptoServiceProvider();

            HashSizeValue = 160;
            base.InitializeKey(key);
        }
    }
}
