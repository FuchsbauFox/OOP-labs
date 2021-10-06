using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Services
{
    public class Shop
    {
        private readonly List<Variety> _varietiesProduct;
        private int _cashbox;

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

        internal void DeliveryProduct(Product product, int quantity, int price)
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

        internal void ChangePrice(Product product, int newPrice)
        {
            Variety variety = FindVariety(product.Id);
            if (variety == null)
            {
                throw new ProductNotExistException();
            }
            else
            {
                variety.ChangePrice(newPrice);
            }
        }

        internal void BuyProducts(Variety variety, int quantity)
        {
            _cashbox += variety.Price * quantity;
            variety.Buy(quantity);
        }

        private static void CheckShopName(string shopName)
        {
            if (shopName == null)
            {
                throw new ArgumentNullException();
            }
        }

        private Variety FindVariety(int id)
        {
            return _varietiesProduct.FirstOrDefault(variety => variety.Id == id);
        }
    }
}