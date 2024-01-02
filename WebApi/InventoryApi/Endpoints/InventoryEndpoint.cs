
namespace InventoryApi.Endpoints
{
    public static class InventoryEndpoint
    {
        public static IEndpointRouteBuilder MapInventoryEndpointEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{productId}/{amount}", GetInventory)
            //.WithName("Get")
            .WithOpenApi();

            return app;
        }       

        private static IResult GetInventory(HttpContext context)
        {
            return Results.Ok();
        }
    }
}
