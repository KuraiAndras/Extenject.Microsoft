using System;
using Zenject;

namespace Extenject.Microsoft
{
    public sealed class ExtenjectServiceProvider : IServiceProvider
    {
        private readonly DiContainer _container;

        public ExtenjectServiceProvider(DiContainer container) => _container= container;

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ZenjectException _)
            {
                return null;
            }
        }
    }
}
