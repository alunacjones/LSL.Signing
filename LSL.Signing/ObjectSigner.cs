using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace LSL.Signing
{
    internal class ObjectSigner : IObjectSigner
    {
        private static HMACSHA256 DefaultSigner = new HMACSHA256(Encoding.UTF8.GetBytes("asdhjkl%6782-yaba-dabah-doo-364$*(!@;"));

        private ObjectSignerBuilder _builder;

        public ObjectSigner(ObjectSignerBuilder builder)
        {
            _builder = builder;
        }

        public byte[] GenerateSignature(object source)
        {
            return (_builder
                .SignatureProvider ?? DefaultSignatureProvider)(
                    (_builder.Serialiser ?? DefaultSerialiser)(source)
                );
        }

        private byte[] DefaultSerialiser(object source)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream()) {
                bf.Serialize(ms, source);
                ms.Flush();
                return ms.GetBuffer();
            }
        }

        private byte[] DefaultSignatureProvider(byte[] arg)
        {
            return DefaultSigner.ComputeHash(arg);
        }
    }
}