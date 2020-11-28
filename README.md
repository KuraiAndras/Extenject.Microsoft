# Extenject.Microsoft [![openupm](https://img.shields.io/npm/v/com.extenject.microsoft?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.extenject.microsoft/)

Use Microsoft.Extensions.DependencyInjection.Abstractions interfaces with Extenject/Zenject.

Implemented types

- IServiceCollection
- IServiceProvider
- IServiceScope
- IServiceScopeFactory

# Installation

Install it via OpenUPM
```pwsh
openupm add com.extenject.microsoft
```

You will also need to provide the Microsoft.Extensions.DependencyInjection.Abstractions DLL for your unity project. This package was tested with the 5.0.0 version, but other versions should also work.

# Usage

Use the DiTranslator, with the Translate extension method on IServiceCollection to translate Microsoft bindings to Extenject/Zenject

```csharp
public sealed class SampleInstaller : MonoInstaller
{
    private readonly IServiceCollection _serviceCollection = new ExtenjectServiceCollection();

    public override void InstallBindings()
    {
        _serviceCollection.AddTransient<IMyService, MyService>();
        _serviceCollection.AddScroped<IService, Service>();
        _serviceCollection.AddSingleton<ISingleton, Singleton>();

        _serviceCollection.Translate(Container); // This will translate the bindings into the provided DiContainer
    }
}
```