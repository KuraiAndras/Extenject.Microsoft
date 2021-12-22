using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using Zenject;

namespace Extenject.Microsoft.Tests
{
    public abstract class InjectionTests : TestBase
    {
        private IServiceCollection Services { get; } = new ExtenjectServiceCollection();
        private IServiceProvider ServiceProvider { get; set; }

        public override void Arrange()
        {
            var container = new DiContainer();

            Services.Translate(container);

            ServiceProvider = container.Resolve<IServiceProvider>();
        }

        public sealed class CanCreateServiceProvider : InjectionTests
        {
            [SetUp]
            public override void Arrange()
            {
                base.Arrange();
            }

            [Test]
            public override void ActAssert() => Assert.IsInstanceOf<ExtenjectServiceProvider>(ServiceProvider);
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
                var (service1, service2) = ServiceProvider.GetRequiredService2<Service>();

                Helper.NotNull(service1, service2);
                Assert.AreNotSame(service1, service2);
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
                var (service1, service2) = ServiceProvider.GetRequiredService2<IService>();

                Helper.NotNull(service1, service2);
                Assert.AreNotSame(service1, service2);
            }
        }

        public sealed class TransientDelegate : InjectionTests
        {
            [SetUp]
            public override void Arrange()
            {
                Services.AddTransient<IService>(_ => new Service());

                base.Arrange();
            }

            [Test]
            public override void ActAssert()
            {
                var (service1, service2) = ServiceProvider.GetRequiredService2<IService>();

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
                var (service1, service2) = ServiceProvider.GetRequiredService2<Service>();

                Helper.NotNull(service1, service2);
                Assert.AreSame(service1, service2);
            }
        }

        public sealed class ImplementationInstanceSingleton : InjectionTests
        {
            private Service _service;

            [SetUp]
            public override void Arrange()
            {
                _service = new Service();
                Services.AddSingleton(_service);

                base.Arrange();
            }

            [Test]
            public override void ActAssert()
            {
                var (service1, service2) = ServiceProvider.GetRequiredService2<Service>();

                Helper.NotNull(service1, service2);
                Assert.AreSame(_service, service1);
                Assert.AreSame(_service, service2);
            }
        }

        public sealed class SingletonDelegate : InjectionTests
        {
            [SetUp]
            public override void Arrange()
            {
                Services.AddSingleton<Service>(_ => new Service());

                base.Arrange();
            }

            [Test]
            public override void ActAssert()
            {
                var (service1, service2) = ServiceProvider.GetRequiredService2<Service>();

                Helper.NotNull(service1, service2);
                Assert.AreSame(service1, service2);
            }
        }

        public sealed class Scoped : InjectionTests
        {
            [SetUp]
            public override void Arrange()
            {
                Services.AddScoped<Service>();

                base.Arrange();
            }

            [Test]
            public override void ActAssert()
            {
                var sameScope1 = ServiceProvider.GetRequiredService<Service>();
                var sameScope2 = ServiceProvider.GetRequiredService<Service>();

                Assert.AreSame(sameScope1, sameScope2);

                var otherScope = ServiceProvider
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope()
                    .ServiceProvider
                    .GetRequiredService<Service>();

                Assert.AreNotSame(sameScope1, otherScope);
                Assert.AreNotSame(sameScope2, otherScope);
            }
        }

        public sealed class ScopedInterface : InjectionTests
        {
            [SetUp]
            public override void Arrange()
            {
                Services.AddScoped<IService, Service>();

                base.Arrange();
            }

            [Test]
            public override void ActAssert()
            {
                var sameScope1 = ServiceProvider.GetRequiredService<IService>();
                var sameScope2 = ServiceProvider.GetRequiredService<IService>();

                Assert.AreSame(sameScope1, sameScope2);

                var scope = ServiceProvider.CreateScope();

                var otherScope = scope.ServiceProvider.GetRequiredService<IService>();

                Assert.AreNotSame(sameScope1, otherScope);
                Assert.AreNotSame(sameScope2, otherScope);
            }
        }

        public sealed class ScopedSingleton : InjectionTests
        {
            private sealed class SingletonService
            {
            }

            [SetUp]
            public override void Arrange()
            {
                Services.AddSingleton<SingletonService>();

                base.Arrange();
            }

            [Test]
            public override void ActAssert()
            {
                var scope = ServiceProvider.CreateScope();

                var singletonService1 = ServiceProvider.GetRequiredService<SingletonService>();
                var singletonService2 = scope.ServiceProvider.GetRequiredService<SingletonService>();

                Assert.AreSame(singletonService1, singletonService2);
            }
        }

        public sealed class ScopedDelegateSingleton : InjectionTests
        {
            private sealed class SingletonService
            {
            }

            [SetUp]
            public override void Arrange()
            {
                Services.AddSingleton<SingletonService>(_ => new SingletonService());

                base.Arrange();
            }

            [Test]
            public override void ActAssert()
            {
                var scope = ServiceProvider.CreateScope();

                var singletonService1 = ServiceProvider.GetRequiredService<SingletonService>();
                var singletonService2 = scope.ServiceProvider.GetRequiredService<SingletonService>();

                Assert.AreSame(singletonService1, singletonService2);
            }
        }

        public sealed class DelegateSingleton : InjectionTests
        {
            [SetUp]
            public override void Arrange()
            {
                Services.AddSingleton<DelegateService>(_ => () => { });

                base.Arrange();
            }

            [Test]
            public override void ActAssert()
            {
                var (service1, service2) = ServiceProvider.GetRequiredService2<DelegateService>();

                Helper.NotNull(service1, service2);
                Assert.AreSame(service1, service2);
            }
        }
    }
}
