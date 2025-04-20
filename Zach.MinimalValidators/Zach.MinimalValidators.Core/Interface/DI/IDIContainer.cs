using Microsoft.Extensions.DependencyInjection;

namespace Zach.MinimalValidators.Core.Interface.DI;

public interface IDIContainer
{
    void AddCollection(IServiceCollection serviceCollection);
};