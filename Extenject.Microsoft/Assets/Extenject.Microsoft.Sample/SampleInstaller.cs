using Microsoft.Extensions.DependencyInjection;
using Zenject;

namespace Extenject.Microsoft.Sample
{
    public sealed class SampleInstaller : MonoInstaller
    {
        private IServiceCollection _services;

        public override void InstallBindings() => _services.Translate(Container);

        public void AddServiceCollection(IServiceCollection services) => _services = services;
    }
}