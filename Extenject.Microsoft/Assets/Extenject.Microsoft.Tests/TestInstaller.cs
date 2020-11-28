using Microsoft.Extensions.DependencyInjection;
using System;
using Zenject;

namespace Extenject.Microsoft.Tests
{
    public sealed class TestInstaller : MonoInstaller
    {
        private readonly IServiceCollection _serviceCollection = new ExtenjectServiceCollection();

        public override void InstallBindings() => _serviceCollection.Translate(Container);

        public IServiceProvider ServiceProvider => Container.Resolve<IServiceProvider>();
    }
}