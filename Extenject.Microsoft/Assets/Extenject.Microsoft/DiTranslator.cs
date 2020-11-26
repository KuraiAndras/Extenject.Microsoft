using System;
using Microsoft.Extensions.DependencyInjection;
using Zenject;

namespace Extenject.Microsoft
{
    public static class DiTranslator
    {
        public static void Translate(this IServiceCollection services, DiContainer container)
        {
            container.Bind<IServiceProvider>().To<ExtenjectServiceProvider>().AsSingle();

            foreach (var service in services)
            {
                if (!(service.ImplementationFactory is null)) throw new NotSupportedException("Factories are not supported");

                var binding = container.Bind(service.ServiceType).To(service.ImplementationType);

                switch (service.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        binding.AsSingle();
                        break;
                    case ServiceLifetime.Scoped:
                        binding.AsCached();
                        break;
                    case ServiceLifetime.Transient:
                        binding.AsTransient();
                        break;
                }
            }
        }
    }
}