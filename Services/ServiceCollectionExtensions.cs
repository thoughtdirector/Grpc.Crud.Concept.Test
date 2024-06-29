using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

public static class ServiceCollectionExtensions
{
    public static void AddAutoServices(this IServiceCollection services, Type interfaceType, Type implementationType)
    {
        var serviceTypes = implementationType.Assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType.GetGenericTypeDefinition()));

        foreach (var type in serviceTypes)
        {
            var serviceInterface = type.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType.GetGenericTypeDefinition());
            services.AddScoped(serviceInterface, type);
        }
    }
}
