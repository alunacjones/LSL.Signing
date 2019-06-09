using System;

namespace LSL.Signing
{
    public class ObjectSignerFactory : IObjectSignerFactory
    {
        public IObjectSigner Build(Action<ObjectSignerBuilder> configurator = null)
        {
            var builder = new ObjectSignerBuilder();
            configurator?.Invoke(builder);

            return new ObjectSigner(builder);
        }
    }
}
