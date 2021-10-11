using System.Collections.Generic;
using System.Linq;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shopping
    {
        private readonly List<ListItem> _shoppingList;
        private readonly List<CartItem> _shoppingCart;

        public Shopping()
        {
            _shoppingList = new List<ListItem>();
            ListOfShopping = _shoppingList;
            _shoppingCart = new List<CartItem>();
        }

        public IReadOnlyList<ListItem> ListOfShopping { get; }

        public void AddProductOnShoppingList(Product product, int quantity)
        {
            CheckProductForAddInList(product);

            _shoppingList.Add(new ListItem(product, quantity));
        }

        public float CalculateCost()
        {
            return _shoppingCart.Sum(item => item.Product.Price * item.QuantityProduct);
        }

        public void MakeBuy()
        {
            foreach (CartItem item in _shoppingCart)
            {
                item.Shop.BuyProducts(item.Product);
            }
        }

        internal void AddProductOnCart(Shop shop, Variety product, int quantity)
        {
            _shoppingCart.Add(new CartItem(shop, product, quantity));
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