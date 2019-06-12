using System.Linq;

namespace LSL.Signing
{
    public static class ObjectSignatureExtensions 
    {
        public static bool Verify(this IObjectSigner source, object signaturee, byte[] expectedSignature)
        {
            return source.GenerateSignature(signaturee).SequenceEqual(expectedSignature);
        }    
    }
}
