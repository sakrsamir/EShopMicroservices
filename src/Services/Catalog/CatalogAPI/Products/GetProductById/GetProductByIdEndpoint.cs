namespace CatalogAPI.Products.GetProductById
{
    // not request just guid
    public record GetProductByIdResponse(Product Product);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid Id,ISender sender) =>
            {
                throw new NotImplementedException();
                //var result = await sender.Send(new GetProductByIdQuery(Id));
                //var response = result.Adapt<GetProductByIdResponse>();
                //return Results.Ok(response);

            }).WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Id")
            .WithDescription("Get Product By Id"); 
        }
    }
}
