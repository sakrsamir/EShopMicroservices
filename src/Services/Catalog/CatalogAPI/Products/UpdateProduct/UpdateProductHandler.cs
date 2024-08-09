namespace CatalogAPI.Products.UpdateProduct
{

    public record UpdateProductCommand (Guid Id,string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(Boolean IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id Is Required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name Is Required").Length(2,250).WithMessage("Name must be more than 2 chars");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category Is Required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile Is Required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price Is Required");
        }
    }
    internal class UpdateProductCommandHandler
        (IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        async Task<UpdateProductResult> IRequestHandler<UpdateProductCommand, UpdateProductResult>.Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductResult From Command{@query}", command);
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(command.Id);
            }

            product.Name = command.Name;
            product.Category = command.Category;
            product.Description = command.Description;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;
            session.Update(product);    
            await session.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(true);
        }
    }
}
