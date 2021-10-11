using System;

namespace Shops.Entities
{
    public class ListItem
    {
        public ListItem(Product product, int quantity)
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