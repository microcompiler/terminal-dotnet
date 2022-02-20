// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Bytewizer.TinyCLR.Security.Cryptography
{
    public class HMACMD5 : HMAC 
    {

        public HMACMD5 () 
            : this (KeyBuilder.Key(8)) 
        {}

        public HMACMD5 (byte[] key) {
            m_hashName = "MD5";
            m_hash1 = new MD5CryptoServiceProvider();
            m_hash2 = new MD5CryptoServiceProvider();

            HashSizeValue = 128;
            base.InitializeKey(key);
        }
    }
}
