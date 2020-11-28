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
                        container
                            .Bind(service.ServiceType)
                            .To(service.ImplementationType)
                            .AsSingle();
                        break;
                    case ServiceLifetime.Scoped:
                        scopedTypes.Add(service);
                        container
                            .Bind(service.ServiceType)
                            .To(service.ImplementationType)
                            .AsSingle();
                        break;
                    case ServiceLifetime.Transient:
                        container
                            .Bind(service.ServiceType)
                            .To(service.ImplementationType)
                            .AsTransient();
                        break;
                }
            }

            return container;
        }
    }
}
