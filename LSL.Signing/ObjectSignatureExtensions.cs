using System.Linq;

namespace LSL.Signing
{
    public static class ObjectSignatureExtensions 
    {
        public static bool Verify(this IObjectSigner source, object signaturee, byte[] expectedSignature)
        {
            var newSig = source.GenerateSignature(signaturee);

            return !source
                .GenerateSignature(signaturee)
                .Select((item, index) => item == expectedSignature[index])
                .Any(i => i == false);
        }    
    }
}
