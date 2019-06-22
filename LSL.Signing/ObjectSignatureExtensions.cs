using System.Linq;

namespace LSL.Signing
{
    /// <summary>
    /// Extensions for the <see cref="IObjectSigner"/> interface
    /// </summary>
    public static class ObjectSignatureExtensions 
    {
        /// <summary>
        /// Verifies that the given object satisifes the given signature
        /// </summary>
        /// <param name="source"><see cref="IObjectSigner"/> instance that the operation is applied to</param>
        /// <param name="signaturee">The object to verify</param>
        /// <param name="expectedSignature">The signature that should be produced</param>
        /// <returns></returns>
        public static bool Verify(this IObjectSigner source, object signaturee, byte[] expectedSignature)
        {
            return source.GenerateSignature(signaturee).SequenceEqual(expectedSignature);
        }    
    }
}
