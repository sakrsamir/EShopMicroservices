namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart):ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(_ => _.Cart).NotNull().WithMessage("card can not be null");
            RuleFor(_ => _.Cart.UserName).NotEmpty().WithMessage("UserName can not be empty");

        }
    }

    public class StoreBasketCommandHandler
        (IBasketRepository repository)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            // store basket in db
            await repository.StoreBasket(command.Cart,cancellationToken);
            // update cache
            return new StoreBasketResult(command.Cart.UserName);
        }
    }
}
