using Contracts.Entities;
using OrderApi.Services;
using ConfigurationManager = Common.Configuration.ConfigurationManager;
using IConfigurationManager = Common.Configuration.IConfigurationManager;

namespace OrderApi.Endpoints
{
    public class OrdersEndpoint
    {
        private readonly IConfigurationManager _configManager;

        public void DefineEndpoint(WebApplication app)
        {
            app.MapPost("/", CreateOrder)
                .WithOpenApi();
        }

        public OrdersEndpoint(IConfigurationManager configManager)
        {
            _configManager = configManager;
        }

        public IResult CreateOrder(IOrdersService service, Order order)
        {
            var orderCreated = service.CreateOrder(order);
            return Results.Ok(orderCreated);
        }

        public void DefineService(IServiceCollection services)
        {
            services.AddSingleton<IConfigurationManager, ConfigurationManager>();
            services.AddSingleton<IOrdersService, OrdersService>();
        }
    }
}
