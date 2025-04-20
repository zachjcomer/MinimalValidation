using System.ComponentModel.DataAnnotations;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Zach.MinimalValidators.Library;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder WithValidationFilter(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilterFactory((filterFactoryContext, next) => {
            var parameters = filterFactoryContext.MethodInfo.GetParameters();
            
            if (!IsValidatable(parameters))
                return (invocationContext) => next(invocationContext);

            return async (invocationContext) =>
            {
                List<ValidationResult> results = [];

                var arguments = invocationContext.Arguments;

                for (int i = 0; i < arguments.Count; i++)
                {
                    var arg = invocationContext.Arguments[i];
                    var param = parameters[i];

                    var attrs = GetAttributesAssignableTo(param, typeof(ValidationAttribute)) ?? [];

                    ValidationContext context = new(arg!);
                    Validator.TryValidateValue(arg, context, results, attrs.OfType<ValidationAttribute>());
                }

                if (results.Any())
                    return Results.BadRequest(results);

                return await next(invocationContext);
            };
        });

        return builder;
    }

    private static IEnumerable<Attribute> GetAttributesAssignableTo(ParameterInfo parameter, Type targetType)
    {
        var customAttribues = parameter.GetCustomAttributes();
        var ret = new List<Attribute>();
        foreach (var custom in customAttribues)
        {
            var t = custom.GetType();

            if (t.IsAssignableTo(targetType))
                ret.Add(custom);
        }

        return ret;
    }

    private static bool IsValidatable(ParameterInfo[] parameters)
        => parameters.Any(x => GetAttributesAssignableTo(x, typeof(ValidationAttribute)).Any());
}
