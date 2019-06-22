using System;
using System.Collections.Generic;

namespace LSL.Signing
{
    /// <summary>
    /// Defines methods for configuration an <see cref="IObjectSigner"/> instance
    /// </summary>
    public sealed class ObjectSignerBuilder
    {
        internal Func<byte[], byte[]> SignatureProvider { get; private set; }
        internal Func<object, byte[]> Serialiser { get; private set; }
        internal ICollection<Action> OnDisposeHandlers { get; private set; } = new List<Action>();
        
        /// <summary>
        /// Set the serialiser for an <see cref="IObjectSigner"/>
        /// </summary>
        /// <param name="serialiser">A delegate to transform an object into a byte array</param>
        /// <returns>The original <see cref="ObjectSignerBuilder"/></returns>
        public ObjectSignerBuilder WithBinarySerialiser(Func<object, byte[]> serialiser)
        {
            Serialiser = serialiser;
            return this;
        }

        /// <summary>
        /// Set the signature creation delegate for an <see cref="IObjectSigner"/>
        /// </summary>
        /// <param name="signatureProvider">A delegate to produce a signature from a byte array</param>
        /// <returns>The original <see cref="ObjectSignerBuilder"/></returns>
        public ObjectSignerBuilder WithSignatureProvider(Func<byte[], byte[]> signatureProvider)
        {
            SignatureProvider = signatureProvider;
            return this;
        }

        /// <summary>
        /// Registers and action to call on disposal of the <see cref="IObjectSigner"/>
        /// </summary>
        /// <param name="toExecuteOnDispose">The action to execute on disposal</param>
        /// <returns>The original <see cref="ObjectSignerBuilder"/></returns>
        public ObjectSignerBuilder OnDispose(Action toExecuteOnDispose) 
        {
            OnDisposeHandlers.Add(toExecuteOnDispose);
            return this;
        }
    }
}