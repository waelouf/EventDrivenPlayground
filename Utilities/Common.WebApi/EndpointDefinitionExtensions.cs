using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Common.WebApi;

public static class EndpointDefinitionExtensions
{
    public static void AddEndpointDefinition(this IServiceCollection services, params Type[] endpointsTypes)
    {
        var endpointDefinitions = new List<IEndpointDefinition>();

        foreach (var endpointType in endpointsTypes)
        {
            endpointDefinitions.AddRange(endpointType.Assembly.ExportedTypes
                .Where(x => typeof(IEndpointDefinition).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
                .Select(Activator.CreateInstance).Cast<IEndpointDefinition>()
                );
        }

        foreach (var item in endpointDefinitions)
        {
            item.DefineService(services);
        }

        services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpointDefinition>);
    }

    public static void UseEndpointDefinitions(this WebApplication app)
    {
        var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>();

        foreach (var endpointDefinition in definitions)
        {
            endpointDefinition.DefineEndpoint(app);
        }
    }
}
