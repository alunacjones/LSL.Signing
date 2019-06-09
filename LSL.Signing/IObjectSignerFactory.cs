using System;

namespace LSL.Signing
{
    public interface IObjectSignerFactory
    {
        IObjectSigner Build(Action<ObjectSignerBuilder> builder = null);
    }
}
