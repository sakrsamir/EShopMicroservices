
using Marten.Pagination;

namespace CatalogAPI.Products.GetProducts
{

    public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10):IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);
    internal class GetProductsQueryHandler
        (IDocumentSession session)
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        async Task<GetProductsResult> IRequestHandler<GetProductsQuery, GetProductsResult>.Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>().ToPagedListAsync(query.PageNumber??1,query.PageSize??10);
            return new GetProductsResult(products);
        }
    }
}
