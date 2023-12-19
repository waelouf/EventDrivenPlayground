using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Common.WebApi;

public interface IEndpointDefinition
{
    void DefineEndpoint(WebApplication app);

    void DefineService(IServiceCollection services);
}
