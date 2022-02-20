namespace Bytewizer.TinyCLR.Security.Cryptography
{
    /// <summary>
    /// Represents the standard parameters for the RSA algorithm.
    /// </summary>
    public struct RSAParameters
    {
        /// <summary>
        /// Private exponent value
        /// </summary>
        public byte[] D;

        /// <summary>
        /// Exponent1 value
        /// </summary>
        public byte[] DP;

        /// <summary>
        /// Exponent2 value
        /// </summary>
        public byte[] DQ;

        /// <summary>
        /// Public exponent value
        /// </summary>
        public byte[] Exponent;

        /// <summary>
        /// Coefficient value
        /// </summary>
        public byte[] InverseQ;

        /// <summary>
        /// Modulus value
        /// </summary>
        public byte[] Modulus;

        /// <summary>
        /// Prime1 value
        /// </summary>
        public byte[] P;

        /// <summary>
        /// Prime2 value
        /// </summary>
        public byte[] Q;
    }
}