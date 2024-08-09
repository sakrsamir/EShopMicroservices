


namespace CatalogAPI.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        :ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);


    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator() 
        { 
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name Is Required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Name Is Required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Name Is Required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Name Is Required");
        }
    }

    internal class CreateProductCommandHandler (IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
           
            // Create Product entity from command
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
            };
            // save database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            // return result 
            return  new CreateProductResult(product.Id);
        }
    }


}
