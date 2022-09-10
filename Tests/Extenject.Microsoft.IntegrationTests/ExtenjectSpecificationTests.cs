using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Specification;
using Zenject;

namespace Extenject.Microsoft.IntegrationTests
{
    public class ExtenjectSpecificationTests : DependencyInjectionSpecificationTests
    {
        protected override IServiceProvider CreateServiceProvider(IServiceCollection serviceCollection)
        {
            var container = new DiContainer();

            serviceCollection.Translate(container);

            return container.Resolve<IServiceProvider>();
        }
    }
}