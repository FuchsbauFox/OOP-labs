using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        private readonly List<Variety> _varietiesProduct;
        private float _cashbox;

        public Shop(string shopName)
        {
            CheckShopName(shopName);

            _varietiesProduct = new List<Variety>();
            VarietiesProductOfShop = _varietiesProduct;
            ShopName = shopName;
            _cashbox = 0;
        }

        public string ShopName { get; }
        internal IReadOnlyList<Variety> VarietiesProductOfShop { get; }

        internal void DeliveryProduct(Product product, int quantity, float price)
        {
            Variety variety = FindVariety(product.Id);
            if (variety == null)
            {
                variety = new Variety(product, quantity, price);
                _varietiesProduct.Add(variety);
            }
            else
            {
                variety.Delivery(quantity);
            }
        }

        internal void ChangePrice(Product product, float newPrice)
        {
            Variety variety = FindVariety(product.Id);
            CheckVariety(product);

            variety.ChangePrice(newPrice);
        }

        internal void BuyProducts(Variety variety)
        {
            _cashbox += variety.Price * variety.QuantityTaken;
            variety.BuySuccessful();
        }

        private static void CheckShopName(string shopName)
        {
            if (shopName == null)
            {
                throw new ArgumentNullException();
            }
        }

        private static void CheckVariety(Product product)
        {
            if (product == null)
            {
                throw new ProductNotExistException();
            }
        }

        private Variety FindVariety(int id)
        {
            return _varietiesProduct.FirstOrDefault(variety => variety.Id == id);
        }
    }
}