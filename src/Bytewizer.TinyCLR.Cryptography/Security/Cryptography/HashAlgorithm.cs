// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
//using System.Diagnostics.Contracts;

namespace Bytewizer.TinyCLR.Security.Cryptography
{
    /// <summary>
    /// Represents the class from which all implementations of cryptographic hash algorithms must derive.
    /// </summary>
    public abstract class HashAlgorithm : IDisposable, ICryptoTransform
    {
        protected int HashSizeValue;
        protected internal byte[] HashValue;
        protected int State = 0;

        private bool m_bDisposed = false;

        protected HashAlgorithm() { }


        /// <summary>
        /// Gets the size, in bits, of the computed hash code.
        /// </summary>
        public virtual int HashSize
        {
            get { return HashSizeValue; }
        }

        /// <summary>
        /// Gets the value of the computed hash code.
        /// </summary>
        public virtual byte[] Hash
        {
            get
            {
                if (m_bDisposed)
                    throw new ObjectDisposedException(null);
                if (State != 0)
                    throw new Exception("Cryptography_HashNotYetFinalized");
                return (byte[])HashValue.Clone();
            }
        }

        static public HashAlgorithm Create()
        {
            return new SHA1CryptoServiceProvider();
        }

        /// <summary>
        /// Computes the hash value for the specified Stream object.
        /// </summary>
        /// <param name="inputStream">The input to compute the hash code for.</param>
        /// <returns>The computed hash code.</returns>
        public byte[] ComputeHash(Stream inputStream)
        {
            if (m_bDisposed)
                throw new ObjectDisposedException(null);

            // Default the buffer size to 4K.
            byte[] buffer = new byte[4096];
            int bytesRead;
            do
            {
                bytesRead = inputStream.Read(buffer, 0, 4096);
                if (bytesRead > 0)
                {
                    HashCore(buffer, 0, bytesRead);
                }
            } while (bytesRead > 0);

            HashValue = HashFinal();
            byte[] Tmp = (byte[])HashValue.Clone();
            Initialize();
            return (Tmp);
        }

        /// <summary>
        /// Computes the hash value for the specified byte array.
        /// </summary>
        /// <param name="buffer">The input to compute the hash code for.</param>
        /// <returns>The computed hash code.</returns>
        public byte[] ComputeHash(byte[] buffer)
        {
            if (m_bDisposed)
                throw new ObjectDisposedException(null);

            // Do some validation
            if (buffer == null) throw new ArgumentNullException("buffer");

            HashCore(buffer, 0, buffer.Length);
            HashValue = HashFinal();
            byte[] Tmp = (byte[])HashValue.Clone();
            Initialize();
            return (Tmp);
        }

        /// <summary>
        /// Computes the hash value for the specified region of the specified byte array.
        /// </summary>
        /// <param name="buffer">The input to compute the hash code for.</param>
        /// <param name="offset">The offset into the byte array from which to begin using data.</param>
        /// <param name="count">The number of bytes in the array to use as data.</param>
        /// <returns>The computed hash code.</returns>
        public byte[] ComputeHash(byte[] buffer, int offset, int count)
        {
            // Do some validation
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "ArgumentOutOfRange_NeedNonNegNum");
            if (count < 0 || (count > buffer.Length))
                throw new ArgumentException("Argument_InvalidValue");
            if ((buffer.Length - count) < offset)
                throw new ArgumentException("Argument_InvalidOffLen");
            //Contract.EndContractBlock();

            if (m_bDisposed)
                throw new ObjectDisposedException(null);

            HashCore(buffer, offset, count);
            HashValue = HashFinal();
            byte[] Tmp = (byte[])HashValue.Clone();
            Initialize();
            return (Tmp);
        }
        /// <summary>
        /// When overridden in a derived class, gets the input block size.
        /// </summary>
        public virtual int InputBlockSize
        {
            get { return (1); }
        }

        /// <summary>
        /// When overridden in a derived class, gets the output block size.
        /// </summary>
        public virtual int OutputBlockSize
        {
            get { return (1); }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether multiple blocks can be transformed.
        /// </summary>
        public virtual bool CanTransformMultipleBlocks
        {
            get { return (true); }
        }

        /// <summary>
        /// Gets a value indicating whether the current transform can be reused.
        /// </summary>
        public virtual bool CanReuseTransform
        {
            get { return (true); }
        }

        /// <summary>
        /// Computes the hash value for the specified region of the input byte array and copies the specified region of the input byte array to the specified region of the output byte array.
        /// </summary>
        /// <param name="inputBuffer">The input to compute the hash code for.</param>
        /// <param name="inputOffset">The offset into the input byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the input byte array to use as data.</param>
        /// <param name="outputBuffer">A copy of the part of the input array used to compute the hash code.</param>
        /// <param name="outputOffset">The offset into the output byte array from which to begin writing data.</param>
        /// <returns>The number of bytes written.</returns>
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            // Do some validation, we let BlockCopy do the destination array validation
            if (inputBuffer == null)
                throw new ArgumentNullException("inputBuffer");
            if (inputOffset < 0)
                throw new ArgumentOutOfRangeException("inputOffset", "ArgumentOutOfRange_NeedNonNegNum");
            if (inputCount < 0 || (inputCount > inputBuffer.Length))
                throw new ArgumentException("Argument_InvalidValue");
            if ((inputBuffer.Length - inputCount) < inputOffset)
                throw new ArgumentException("Argument_InvalidOffLen");
            //Contract.EndContractBlock();

            if (m_bDisposed)
                throw new ObjectDisposedException(null);

            // Change the State value
            State = 1;
            HashCore(inputBuffer, inputOffset, inputCount);
            if ((outputBuffer != null) && ((inputBuffer != outputBuffer) || (inputOffset != outputOffset)))
                Array.Copy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
            return inputCount;
        }

        /// <summary>
        /// Computes the hash value for the specified region of the specified byte array.
        /// </summary>
        /// <param name="inputBuffer">The input to compute the hash code for.</param>
        /// <param name="inputOffset">The offset into the byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the byte array to use as data.</param>
        /// <returns>An array that is a copy of the part of the input that is hashed.</returns>
        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            // Do some validation
            if (inputBuffer == null)
                throw new ArgumentNullException("inputBuffer");
            if (inputOffset < 0)
                throw new ArgumentOutOfRangeException("inputOffset", "ArgumentOutOfRange_NeedNonNegNum");
            if (inputCount < 0 || (inputCount > inputBuffer.Length))
                throw new ArgumentException("Argument_InvalidValue");
            if ((inputBuffer.Length - inputCount) < inputOffset)
                throw new ArgumentException("Argument_InvalidOffLen");
            //Contract.EndContractBlock();

            if (m_bDisposed)
                throw new ObjectDisposedException(null);

            HashCore(inputBuffer, inputOffset, inputCount);
            HashValue = HashFinal();
            byte[] outputBytes;
            if (inputCount != 0)
            {
                outputBytes = new byte[inputCount];
                Array.Copy(inputBuffer, inputOffset, outputBytes, 0, inputCount);
            }
            else
            {
                outputBytes = new byte[0];  // EmptyArray<Byte>.Value;
            }
            // reset the State value
            State = 0;
            return outputBytes;
        }

        // IDisposable methods

        // To keep mscorlib compatibility with Orcas, CoreCLR's HashAlgorithm has an explicit IDisposable
        // implementation. Post-Orcas the desktop has an implicit IDispoable implementation.
#if FEATURE_CORECLR
        void IDisposable.Dispose()
        {
            Dispose();
        }
#endif // FEATURE_CORECLR

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Clear()
        {
            (this as IDisposable).Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (HashValue != null)
                    Array.Clear(HashValue, 0, HashValue.Length);
                HashValue = null;
                m_bDisposed = true;
            }
        }

        /// <summary>
        /// Initializes an implementation of the HashAlgorithm class.
        /// </summary>
        public abstract void Initialize();

        protected abstract void HashCore(byte[] array, int ibStart, int cbSize);

        protected abstract byte[] HashFinal();
    }
}
