using System;
using Zenject;

namespace Extenject.Microsoft
{
    public sealed class ExtenjectServiceProvider : IServiceProvider
    {
        private readonly DiContainer _container;

        public ExtenjectServiceProvider(DiContainer container) => _container= container;

        public object GetService(Type serviceType) => _container.Resolve(serviceType);
    }
}
