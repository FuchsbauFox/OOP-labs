using System;

namespace Shops.Entities
{
    public class Product
    {
        public Product(string productName, int productId)
        {
            CheckProductName(productName);
            CheckProductId(productId);

            Name = productName;
            Id = productId;
        }

        public string Name { get; }
        public int Id { get; }

        private static void CheckProductName(string productName)
        {
            if (productName == null)
            {
                throw new ArgumentNullException();
            }
        }

        private static void CheckProductId(int productId)
        {
            if (productId <= 0)
            {
                throw new AggregateException();
            }
        }
    }
}