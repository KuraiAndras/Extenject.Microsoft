using Microsoft.Extensions.DependencyInjection;
using System;
using Zenject;

namespace Extenject.Microsoft.Tests
{
    public sealed class TestInstaller : MonoInstaller
    {
        public static IServiceCollection Services { get; set; }

        public override void InstallBindings() => Services.Translate(Container);

        public IServiceProvider ServiceProvider => Container.Resolve<IServiceProvider>();
    }
}