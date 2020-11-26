using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Extenject.Microsoft
{
    public sealed class ExtenjectServiceScope : IServiceScope, IServiceProvider
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private readonly IServiceProvider _serviceProvider;

        public ExtenjectServiceScope(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public void Dispose() => _disposables.ForEach(d => d.Dispose());

        public IServiceProvider ServiceProvider => this;

        public object GetService(Type serviceType)
        {
            var instance = _serviceProvider.GetService(serviceType);

            if (instance is IDisposable d) _disposables.Add(d);

            return instance;
        }
    }
}