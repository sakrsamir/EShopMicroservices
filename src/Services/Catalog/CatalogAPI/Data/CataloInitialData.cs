using Marten.Schema;

namespace CatalogAPI.Data
{
    public class CataloInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync())
                return;
            session.Store<Product>(GetPreconfiguredProducts());
            await session.SaveChangesAsync();
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "IPhone X",
                    Description = "this phone desc",
                    ImageFile = "image",
                    Price = 950.00M,
                    Category = new List<string>{"Smart Phone"}
                }
            };
        }
    }
}
