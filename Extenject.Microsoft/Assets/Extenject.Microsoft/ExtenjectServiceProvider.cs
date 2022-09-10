using System;
using Zenject;

namespace Extenject.Microsoft
{
    public sealed class ExtenjectServiceProvider : IServiceProvider, IDisposable
    {
        private readonly DiContainer _container;

        public ExtenjectServiceProvider(DiContainer container) => _container = container;

        public void Dispose()
        {
        }

        public object GetService(Type serviceType) => _container.Resolve(serviceType);
    }
}
