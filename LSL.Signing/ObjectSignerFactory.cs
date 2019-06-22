using System;

namespace LSL.Signing
{
    /// <inheritdoc/>
    public class ObjectSignerFactory : IObjectSignerFactory
    {
        /// <inheritdoc/>
        public IObjectSigner Build(Action<ObjectSignerBuilder> configurator = null)
        {
            var builder = new ObjectSignerBuilder();
            configurator?.Invoke(builder);

            return new ObjectSigner(builder);
        }
    }
}
