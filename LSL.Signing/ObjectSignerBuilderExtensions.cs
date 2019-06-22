using System;
using System.Security.Cryptography;
using System.Text;

namespace LSL.Signing
{
    /// <summary>
    /// Builder extensions
    /// </summary>
    public static class ObjectSignerBuilderExtensions 
    {        
        private static ObjectSignerBuilder AddDisposableSignatureProvider(ObjectSignerBuilder source, HashAlgorithm algorithm)
        {
            return source
                .WithSignatureProvider(algorithm.ComputeHash)
                .OnDispose(() => algorithm.Dispose());
        }

        /// <summary>
        /// Use the HMACSHA256 ComputeHash method for signature generation
        /// </summary>
        /// <param name="source"><see cref="ObjectSignerBuilder"/> instance that the operation is applied to</param>
        /// <param name="secret">Secret key for the HMAC instance to use</param>
        /// <returns><see cref="ObjectSignerBuilder"/> instance to fluently build with</returns>
        public static ObjectSignerBuilder WithHmac256SignatureProvider(this ObjectSignerBuilder source, byte[] secret)
        {
            return AddDisposableSignatureProvider(source, new HMACSHA256(secret));
        }

        /// <summary>
        /// Use the HMACSHA384 ComputeHash method for signature generation
        /// </summary>
        /// <param name="source"><see cref="ObjectSignerBuilder"/> instance that the operation is applied to</param>
        /// <param name="secret">Secret key for the HMAC instance to use</param>
        /// <returns><see cref="ObjectSignerBuilder"/> instance to fluently build with</returns>
        public static ObjectSignerBuilder WithHmac384SignatureProvider(this ObjectSignerBuilder source, byte[] secret)
        {
            return AddDisposableSignatureProvider(source, new HMACSHA384(secret));
        }        

        /// <summary>
        /// Use the HMACSHA512 ComputeHash method for signature generation
        /// </summary>
        /// <param name="source"><see cref="ObjectSignerBuilder"/> instance that the operation is applied to</param>
        /// <param name="secret">Secret key for the HMAC instance to use</param>
        /// <returns><see cref="ObjectSignerBuilder"/> instance to fluently build with</returns>
        public static ObjectSignerBuilder WithHmac512SignatureProvider(this ObjectSignerBuilder source, byte[] secret)
        {
            return AddDisposableSignatureProvider(source, new HMACSHA512(secret));
        }

        /// <summary>
        /// Setup a delegate to convert an object to a string
        /// </summary>
        /// <param name="source"><see cref="ObjectSignerBuilder"/> instance that the operation is applied to</param>
        /// <param name="objectToStringSerialiser">Delegate to serialise an object into a string</param>
        /// <returns></returns>
        public static ObjectSignerBuilder WithStringBasedSerialiser(this ObjectSignerBuilder source, Func<object, string> objectToStringSerialiser) 
        {
            source.WithBinarySerialiser(input => Encoding.UTF8.GetBytes(objectToStringSerialiser(input)));
            return source;
        }
    }
}