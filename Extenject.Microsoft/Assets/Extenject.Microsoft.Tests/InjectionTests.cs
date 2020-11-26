using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using Zenject;

namespace Extenject.Microsoft.Tests
{
    public abstract class InjectionTests : TestBase
    {
        private DiContainer Container { get; } = new DiContainer();
        private IServiceCollection Services { get; } = new ExtenjectServiceCollection();

        public override void Arrange() => Services.Translate(Container);

        public sealed class CanCreateServiceProvider : InjectionTests
        {
            [SetUp]
            public override void Arrange()
            {
                base.Arrange();
            }

            [Test]
            public override void ActAssert()
            {
                var serviceProvider = Container.Resolve<IServiceProvider>();

                Assert.IsInstanceOf<ExtenjectServiceProvider>(serviceProvider);
            }
        }

        public sealed class TransientInterface : InjectionTests
        {
            [SetUp]
            public override void Arrange()
            {
                Services.AddTransient<IService, Service>();

                base.Arrange();
            }

            [Test]
            public override void ActAssert()
            {
                var serviceProvider = Container.Resolve<IServiceProvider>();

                var (service1, service2) = serviceProvider.GetRequiredService2<IService>();

                Helper.NotNull(service1, service2);
                Assert.AreNotSame(service1, service2);
            }
        }

        public sealed class Singleton : InjectionTests
        {
            [SetUp]
            public override void Arrange()
            {
                Services.AddSingleton<Service>();

                base.Arrange();
            }

            [Test]
            public override void ActAssert()
            {
                var serviceProvider = Container.Resolve<IServiceProvider>();

                var (service1, service2) = serviceProvider.GetRequiredService2<Service>();

                Helper.NotNull(service1, service2);
                Assert.AreSame(service1, service2);
            }
        }

        public sealed class Transient : InjectionTests
        {
            [SetUp]
            public override void Arrange()
            {
                Services.AddTransient<Service>();

                base.Arrange();
            }

            [Test]
            public override void ActAssert()
            {
                var serviceProvider = Container.Resolve<IServiceProvider>();

                var (service1, service2) = serviceProvider.GetRequiredService2<Service>();

                Helper.NotNull(service1, service2);
                Assert.AreNotSame(service1, service2);
            }
        }
    }
}
