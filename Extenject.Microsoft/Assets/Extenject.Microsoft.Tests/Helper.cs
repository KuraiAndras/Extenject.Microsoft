using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace Extenject.Microsoft.Tests
{
    public static class Helper
    {
        public static void NotNull(params object[] objects)
        {
            foreach (var o in objects)
            {
                Assert.That(o, Is.Not.Null);
            }
        }

        public static (T service1, T service2) GetRequiredService2<T>(this IServiceProvider serviceProvider) =>
            (serviceProvider.GetRequiredService<T>(), serviceProvider.GetRequiredService<T>());
    }
}