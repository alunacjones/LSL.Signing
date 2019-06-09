namespace LSL.Signing
{
    public interface IObjectSigner
    {
        byte[] GenerateSignature(object source);
    }
}