namespace LSL.Signing
{
    /// <summary>
    /// Defines the operations that an <see cref="IObjectSigner"/> can perform
    /// </summary>
    public interface IObjectSigner
    {
        /// <summary>
        /// Generate a signature for the given object
        /// </summary>
        /// <param name="source">The object whose signature will be generated</param>
        /// <returns></returns>
        byte[] GenerateSignature(object source);
    }
}