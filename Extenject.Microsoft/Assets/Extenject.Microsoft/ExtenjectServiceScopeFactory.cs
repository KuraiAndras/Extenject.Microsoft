using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Zenject;

namespace Extenject.Microsoft
{
    public sealed class ExtenjectServiceScopeFactory : IServiceScopeFactory
    {
        private readonly DiContainer _container;
        private readonly IReadOnlyCollection<ServiceDescriptor> _scopedTypes;

        public ExtenjectServiceScopeFactory(DiContainer container, IReadOnlyCollection<ServiceDescriptor> scopedTypes)
        {
            _container = container;
            _scopedTypes = scopedTypes;
        }

        public IServiceScope CreateScope()
        {
            var container = new DiContainer(_container);

            foreach (var service in _scopedTypes)
            {
                container.Rebind(service.ServiceType)
                    .To(service.ImplementationType)
                    .AsSingle();
            }

            return new ExtenjectServiceScope(new ExtenjectServiceProvider(container));
        }
    }
}