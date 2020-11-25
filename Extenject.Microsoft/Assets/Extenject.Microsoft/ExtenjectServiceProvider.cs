using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace Extenject.Microsoft
{
    public sealed class ExtenjectServiceProvider : IServiceProvider
    {
        private readonly DiContainer _container;

        public ExtenjectServiceProvider(DiContainer sp) => _container = sp;

        public object GetService(Type serviceType) => _container.Resolve(serviceType);
    }

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

    public sealed class ExtenjectServiceCollection : IServiceCollection
    {
        private readonly List<ServiceDescriptor> _services = new List<ServiceDescriptor>();

        public IEnumerator<ServiceDescriptor> GetEnumerator() => _services.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(ServiceDescriptor item) => _services.Add(item);

        public void Clear() => _services.Clear();

        public bool Contains(ServiceDescriptor item) => _services.Contains(item);

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex) => _services.CopyTo(array, arrayIndex);

        public bool Remove(ServiceDescriptor item) => _services.Remove(item);

        public int Count => _services.Count;
        public bool IsReadOnly { get; } = false;

        public int IndexOf(ServiceDescriptor item) => _services.IndexOf(item);

        public void Insert(int index, ServiceDescriptor item) => _services.Insert(index, item);

        public void RemoveAt(int index) => _services.RemoveAt(index);

        public ServiceDescriptor this[int index]
        {
            get => _services[index];
            set => _services[index] = value;
        }
    }
}
