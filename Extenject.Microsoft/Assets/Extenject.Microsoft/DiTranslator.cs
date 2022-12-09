using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Extensions.DependencyInjection;

using Zenject;

namespace Extenject.Microsoft
{
    public static class DiTranslator
    {
        public static DiContainer Translate(this IServiceCollection services, DiContainer container)
        {
            container
                .Bind<IServiceProvider>()
                .To<ExtenjectServiceProvider>()
                .AsSingle();

            var scopedTypes = new List<ServiceDescriptor>();

            container
                .Bind<IServiceScopeFactory>()
                .FromMethod(ctx => new ExtenjectServiceScopeFactory(ctx.Container, new ReadOnlyCollection<ServiceDescriptor>(scopedTypes)))
                .Lazy();

            foreach (var service in services)
            {
                if (service.Lifetime == ServiceLifetime.Scoped)
                {
                    scopedTypes.Add(service);
                }

                if (!(service.ImplementationFactory is null))
                {
                    BindToImplementationFactory(container, service);
                }
                else
                {
                    BindToType(container, service);
                }
            }

            return container;
        }

        private static DiContainer BindToImplementationFactory(DiContainer container, ServiceDescriptor service)
        {
            switch (service.Lifetime)
            {
                case ServiceLifetime.Transient:
                    container.Bind(service.ServiceType)
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

        private static DiContainer BindToType(DiContainer container, ServiceDescriptor service)
        {
            switch (service.Lifetime)
            {
                case ServiceLifetime.Transient:
                    container
                        .Bind(service.ServiceType)
                        .To(service.ImplementationType ?? service.ServiceType)
                        .AsTransient();
                    break;
                case ServiceLifetime.Singleton:
                case ServiceLifetime.Scoped:
                default:
                    BindSingleton(container, service);
                    break;
            }

            return container;
        }

        private static DiContainer BindSingleton(DiContainer container, ServiceDescriptor service)
        {
            if (service.ImplementationInstance == null)
            {
                container
                    .Bind(service.ServiceType)
                    .To(service.ImplementationType ?? service.ServiceType)
                    .AsSingle();
            }
            else
            {
                //Bind it to the existing instance
                container
                    .Bind(service.ServiceType)
                    .To(service.ImplementationType ?? service.ServiceType)
                    .FromInstance(service.ImplementationInstance);
            }

            return container;
        }
    }
}
