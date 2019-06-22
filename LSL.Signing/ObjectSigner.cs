using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace LSL.Signing
{
    /// <inheritdoc/>
    internal class ObjectSigner : IObjectSigner
    {
        private static readonly HMACSHA256 DefaultSigner = new HMACSHA256(Encoding.UTF8.GetBytes("asdhjkl%6782-yaba-dabah-doo-364$*(!@;"));

        private readonly ObjectSignerBuilder _builder;
        private bool _disposed = false;
        
        public ObjectSigner(ObjectSignerBuilder builder)
        {
            _builder = builder;
        }

        /// <inheritdoc/>
        public byte[] GenerateSignature(object source)
        {
            return (_builder
                .SignatureProvider ?? DefaultSignatureProvider)(
                    (_builder.Serialiser ?? DefaultSerialiser)(source)
                );
        }
        
        /// <inheritdoc/>
        public void Dispose()
        { 
            Dispose(true);
            GC.SuppressFinalize(this);           
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return; 
            
            if (disposing) {
                foreach (var handler in _builder.OnDisposeHandlers)
                {
                    handler?.Invoke();
                }
            }
            
            _disposed = true;
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