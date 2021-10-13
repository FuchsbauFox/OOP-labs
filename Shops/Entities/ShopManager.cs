using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Entities
{
    public class ShopManager
    {
        private readonly List<Shop> _shops;
        private readonly List<Product> _catalogProducts;
        private int _idLastProduct;

        public ShopManager()
        {
            _shops = new List<Shop>();
            _catalogProducts = new List<Product>();
            _idLastProduct = 1;
        }

        public Shop AddShop(string shopName)
        {
            var shop = new Shop(shopName);
            CheckShopOnExist(shop);

            _shops.Add(shop);
            return shop;
        }

        public Product AddProduct(string productName)
        {
            var product = new Product(productName, _idLastProduct++);
            CheckProductOnExist(product);

            _catalogProducts.Add(product);
            return product;
        }

        public void DeliverGoodsInShop(Shop shop, Product product, int quantity, float price = 0)
        {
            CheckShopOnExist(shop, true);
            CheckProductOnExist(product, true);

            shop.DeliveryProduct(product, quantity, price);
        }

        public void ChangePriceProductInShop(Shop shop, Product product, int newPrice)
        {
            CheckShopOnExist(shop, true);
            CheckProductOnExist(product, true);

            shop.ChangePrice(product, newPrice);
        }

        public void CreatCart(Shopping shopping)
        {
            foreach (ListItem item in shopping.ListOfShopping)
            {
                CheckProductOnExist(item.Product, true);

                int neededQuantity = item.Quantity;
                while (neededQuantity != 0)
                {
                    Variety productWithBestPrice = null;
                    Shop shopWithBestPrice = null;
                    foreach (Shop shop in _shops)
                    {
                        foreach (Variety variety in shop.VarietiesProductOfShop)
                        {
                            if (variety.Id != item.Product.Id || variety.Quantity == variety.QuantityTaken ||
                                (productWithBestPrice != null && productWithBestPrice.Price <= variety.Price)) continue;
                            productWithBestPrice = variety;
                            shopWithBestPrice = shop;
                        }
                    }

                    CheckProductWithBestPrice(productWithBestPrice);
                    if (neededQuantity < productWithBestPrice.Quantity)
                    {
                        shopping.AddProductOnCart(shopWithBestPrice, productWithBestPrice, neededQuantity);
                        productWithBestPrice.TakenProducts(neededQuantity);
                        neededQuantity = 0;
                    }
                    else
                    {
                        neededQuantity -= productWithBestPrice.Quantity;
                        shopping.AddProductOnCart(shopWithBestPrice, productWithBestPrice, productWithBestPrice.Quantity);
                        productWithBestPrice.TakenProducts(productWithBestPrice.Quantity);
                    }
                }
            }
        }

        private static void CheckProductWithBestPrice(Variety product)
        {
            if (product == null)
            {
                throw new NotEnoughProductsInShopsException();
            }
        }

        private void CheckProductOnExist(Product verifiableProduct, bool shouldExist = false)
        {
            bool productExist = _catalogProducts.Any(product => product.Name == verifiableProduct.Name);
            switch (shouldExist)
            {
                case false when productExist:
                    throw new ProductAlreadyExistException();
                case true when !productExist:
                    throw new ProductNotExistException();
            }
        }

        private void CheckShopOnExist(Shop verifiableShop, bool shouldExist = false)
        {
            bool shopExist = _shops.Any(shop => shop.ShopName == verifiableShop.ShopName);
            switch (shouldExist)
            {
                case false when shopExist:
                    throw new ShopAlreadyExistException();
                case true when !shopExist:
                    throw new ShopNotExistException();
            }
        }
    }
}