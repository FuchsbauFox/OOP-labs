using System;

namespace Shops.Services
{
    public class ItemShoppingList
    {
        public ItemShoppingList(Product product, int quantity)
        {
            CheckQuantity(quantity);

            Product = product;
            Quantity = quantity;
        }

        public Product Product { get; }
        public int Quantity { get; }

        private static void CheckQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException();
            }
        }
    }
}