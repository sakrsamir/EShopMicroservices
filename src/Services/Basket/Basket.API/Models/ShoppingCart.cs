namespace Basket.API.Models
{
    public class ShoppingCart
    {
        public string UserName { get; set; } = default!;
        public List<ShoppingCartItem> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(_=>_.Price * _.Quantity);

        public ShoppingCart(string userName)
        {
            UserName = userName ;
        }


        // Required For mapping
        public ShoppingCart()
        {
                
        }
    }
}
