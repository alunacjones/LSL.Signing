using System;

namespace LSL.Signing
{
    /// <summary>
    /// Facilitates the creation of <see cref="IObjectSigner"/> instances
    /// </summary>
    public interface IObjectSignerFactory
    {
        /// <summary>
        /// Builds a new <see cref="IObjectSigner"/> instance with an optional builder
        /// </summary>
        /// <param name="builder">A delegate that is provided an ObjectSignerBuilder for configuring the <see cref="IObjectSigner"/> instance</param>
        /// <returns></returns>
        IObjectSigner Build(Action<ObjectSignerBuilder> builder = null);
    }
}
