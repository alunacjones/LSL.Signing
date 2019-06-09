using System;

namespace LSL.Signing
{
    public class ObjectSignerBuilder
    {
        internal Func<byte[], byte[]> SignatureProvider { get; private set; }
        internal Func<object, byte[]> Serialiser { get; private set; }

        public ObjectSignerBuilder WithBinarySerialiser(Func<object, byte[]> serialiser)
        {
            Serialiser = serialiser;
            return this;
        }

        public ObjectSignerBuilder WithSignatureProvider(Func<byte[], byte[]> signatureProvider)
        {
            SignatureProvider = signatureProvider;
            return this;
        }
    }
}