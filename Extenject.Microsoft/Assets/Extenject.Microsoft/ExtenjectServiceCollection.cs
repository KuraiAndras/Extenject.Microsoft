using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Extenject.Microsoft
{
    public sealed class ExtenjectServiceCollection : List<ServiceDescriptor>, IServiceCollection
    {
    }
}