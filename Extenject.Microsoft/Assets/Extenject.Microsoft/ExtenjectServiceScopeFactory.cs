using System;
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
                if (service.ImplementationFactory is null)
                {
                    RebindToType(container, service);
                }
                else
                {
                    RebindToImplementationFactory(container, service);
                }
            }

            return new ExtenjectServiceScope(new ExtenjectServiceProvider(container));
        }

        private static DiContainer RebindToImplementationFactory(DiContainer container, ServiceDescriptor service)
        {
            switch (service.Lifetime)
            {
                case ServiceLifetime.Transient:
                    container.Rebind(service.ServiceType)
                        .FromMethodUntyped(ctx => service.ImplementationFactory(ctx.Container.Resolve<IServiceProvider>())).AsTransient();
                    break;
                case ServiceLifetime.Scoped:
                case ServiceLifetime.Singleton:
                default:
                    container.Bind(service.ServiceType)
                        .FromMethodUntyped(ctx => service.ImplementationFactory(ctx.Container.Resolve<IServiceProvider>())).AsSingle();
                    break;
            }

            return container;
        }

        private static DiContainer RebindToType(DiContainer container, ServiceDescriptor service)
        {
            switch (service.Lifetime)
            {
                case ServiceLifetime.Transient:
                    container
                        .Rebind(service.ServiceType)
                        .To(service.ImplementationType ?? service.ServiceType)
                        .AsTransient();
                    break;
                case ServiceLifetime.Singleton:
                case ServiceLifetime.Scoped:
                default:
                    RebindSingleton(container, service);
                    break;
            }

            return container;
        }

        private static DiContainer RebindSingleton(DiContainer container, ServiceDescriptor service)
        {
            if (service.ImplementationInstance == null)
            {
                container
                    .Rebind(service.ServiceType)
                    .To(service.ImplementationType ?? service.ServiceType)
                    .AsSingle();
            }
            else
            {
                //Bind it to the existing instance
                container
                    .Rebind(service.ServiceType)
                    .To(service.ImplementationType ?? service.ServiceType)
                    .FromInstance(service.ImplementationInstance);
            }

            return container;
        }
    }
}