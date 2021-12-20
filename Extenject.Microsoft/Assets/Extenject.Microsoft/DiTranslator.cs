using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                if (!(service.ImplementationFactory is null))
                {
                    container.Bind(service.ServiceType)
                        .FromMethodUntyped(ctx => service.ImplementationFactory(ctx.Container.Resolve<IServiceProvider>()));

                    return container;
                }


                switch (service.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        BindSingleton(container, service);
                        break;
                    case ServiceLifetime.Scoped:
                        scopedTypes.Add(service);
                        BindSingleton(container, service);
                        break;
                    case ServiceLifetime.Transient:
                        container
                            .Bind(service.ServiceType)
                            .To(service.ImplementationType ?? service.ServiceType)
                            .AsTransient();
                        break;
                }
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
