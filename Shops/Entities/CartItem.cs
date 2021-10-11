using System;

namespace Shops.Entities
{
    internal class CartItem
    {
        internal CartItem(Shop shop, Variety product, int quantityProduct)
        {
            CheckQuantity(quantityProduct);

            Shop = shop;
            Product = product;
            QuantityProduct = quantityProduct;
        }

        public Shop Shop { get; }
        public Variety Product { get; }
        public int QuantityProduct { get; }

        private static void CheckQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException();
            }
        }
    }
}