using Microsoft.Extensions.DependencyInjection;

using Zach.MinimalValidators.Core.Implementation.Exceptions;
using Zach.MinimalValidators.Core.Interface.DI;
using Zach.MinimalValidators.Core.Interface.Exceptions;

namespace Zach.MinimalValidators.Core.Implementation.DI;

public class DIContainer : IDIContainer
{
    public void AddCollection(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IExceptionService, ExceptionService>();
    }
}