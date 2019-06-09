using System;
using System.Security.Cryptography;
using System.Text;

namespace LSL.Signing
{
    public static class ObjectSignerBuilderExtensions 
    {
        public static ObjectSignerBuilder WithHmac256SignatureProvider(this ObjectSignerBuilder source, byte[] secret)
        {
            source.WithSignatureProvider(new HMACSHA256(secret).ComputeHash);
            return source;
        }

        public static ObjectSignerBuilder WithHmac384SignatureProvider(this ObjectSignerBuilder source, byte[] secret)
        {
            source.WithSignatureProvider(new HMACSHA384(secret).ComputeHash);
            return source;
        }        

        public static ObjectSignerBuilder WithHmac512SignatureProvider(this ObjectSignerBuilder source, byte[] secret)
        {
            source.WithSignatureProvider(new HMACSHA512(secret).ComputeHash);
            return source;
        }
        
        public static ObjectSignerBuilder WithRsa(this ObjectSignerBuilder source) 
        {
            var rsaProvider = new RSACryptoServiceProvider();
            source.WithSignatureProvider(input => rsaProvider.SignData(input, 0, input.Length, null));
            return source;
        }

        public static ObjectSignerBuilder WithStringBasedSerialiser(this ObjectSignerBuilder source, Func<object, string> objectToStringSerialiser) 
        {
            source.WithBinarySerialiser(input => Encoding.UTF8.GetBytes(objectToStringSerialiser(input)));
            return source;
        }
    }
}