using Microsoft.Extensions.DependencyInjection;

using Zach.MinimalValidators.Core.Interface.DI;

namespace Zach.MinimalValidators.Core.Extensions.AspNetCore;

public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds services registered in the container to the given IServiceCollection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddContainer<T>(this IServiceCollection services)
    where T : IDIContainer, new()
    {
        T container = new();
        container.AddCollection(services);

        return services;
    }
}