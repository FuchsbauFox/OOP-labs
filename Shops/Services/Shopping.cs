using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Services
{
    public class Shopping
    {
        private readonly List<ItemShoppingList> _shoppingList;
        private readonly List<ItemShoppingCart> _shoppingCart;

        public Shopping()
        {
            _shoppingList = new List<ItemShoppingList>();
            ListOfShopping = _shoppingList;
            _shoppingCart = new List<ItemShoppingCart>();
        }

        public IReadOnlyList<ItemShoppingList> ListOfShopping { get; }

        public void AddProductOnShoppingList(Product product, int quantity)
        {
            CheckProductForAddInList(product);

            _shoppingList.Add(new ItemShoppingList(product, quantity));
        }

        public int CalculateCost()
        {
            int cost = 0;
            foreach (ItemShoppingCart item in _shoppingCart)
            {
                cost += item.Product.Price * item.QuantityProduct;
            }

            return cost;
        }

        public void MakeBuy()
        {
            foreach (ItemShoppingCart item in _shoppingCart)
            {
                item.Shop.BuyProducts(item.Product, item.QuantityProduct);
            }
        }

        internal void AddProductOnCart(Shop shop, Variety product, int quantity)
        {
            _shoppingCart.Add(new ItemShoppingCart(shop, product, quantity));
        }

        private void CheckProductForAddInList(Product newProduct)
        {
            if (_shoppingList.Any(item => item.Product == newProduct))
            {
                throw new ProductAlreadyAddInShoppingListException();
            }
        }
    }
}