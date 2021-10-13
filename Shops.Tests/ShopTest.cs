using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shops.Entities;
using Shops.Tools;

namespace Shops.Tests
{
    public class Tests
    {
        private ShopManager _manager;
        private Customer _customer;

        [SetUp]
        public void Setup()
        {
            _manager = new ShopManager();
            _customer = new Customer();
        }

        [Test]
        public void CreateShopsWithSameName_throwException()
        {
            Assert.Catch<ShopsException>(() =>
            {
                Shop shop1 = _manager.AddShop("5");
                Shop shop2 = _manager.AddShop("5");
            });
        }
        
        [Test]
        public void CreateProductsWithSameName_throwException()
        {
            Assert.Catch<ShopsException>(() =>
            {
                Product product1 = _manager.AddProduct("milk");
                Product product2 = _manager.AddProduct("milk");
            });
        }
        
        
        
        [Test]
        public void CreateShopAndCatalog_DeliveryAndBuyProduct()
        {
            Product product1 = _manager.AddProduct("milk");
            Shop shop1 = _manager.AddShop("5");
            _manager.DeliverGoodsInShop(shop1, product1, 10, 5);
            _manager.DeliverGoodsInShop(shop1, product1, 10);
            _customer.Shopping.AddProductOnShoppingList(product1, 5);
            _manager.CreatCart(_customer.Shopping);
            _customer.Pay();
        }

        [Test]
        public void ChangePriceOnProduct()
        {
            Product product1 = _manager.AddProduct("milk");
            Shop shop1 = _manager.AddShop("5");
            _manager.DeliverGoodsInShop(shop1, product1, 10, 5);
            _manager.ChangePriceProductInShop(shop1, product1, 20);
        }

        [Test]
        public void BuySeveralProductsNotEnoughProduct_throwException()
        {
            Product product1 = _manager.AddProduct("milk");
            Shop shop1 = _manager.AddShop("5");
            Shop shop2 = _manager.AddShop("FixPrice");
            
            _manager.DeliverGoodsInShop(shop1, product1, 10, 5);
            
            _customer.Shopping.AddProductOnShoppingList(product1, 1000);
            Assert.Catch<ShopsException>(() =>
            {
                _manager.CreatCart(_customer.Shopping);
                _customer.Pay();
            });
        }
        
        [Test]
        public void BuySeveralProductsUserHasEnoughMoneyTransferMoney()
        {
            Product product1 = _manager.AddProduct("milk");
            Product product2 = _manager.AddProduct("eggs");
            Product product3 = _manager.AddProduct("bread");
            Shop shop1 = _manager.AddShop("5");
            Shop shop2 = _manager.AddShop("FixPrice");
            
            _manager.DeliverGoodsInShop(shop1, product1, 10, 5);
            _manager.DeliverGoodsInShop(shop2, product1, 10, 100000);
            _manager.DeliverGoodsInShop(shop1, product2, 10, 2);
            _manager.DeliverGoodsInShop(shop1, product3, 10, 3);
            
            _customer.Shopping.AddProductOnShoppingList(product1, 10);
            _customer.Shopping.AddProductOnShoppingList(product2, 10);
            _customer.Shopping.AddProductOnShoppingList(product3, 10);
            _manager.CreatCart(_customer.Shopping);
            _customer.Pay();
        }
        
        [Test]
        public void BuySeveralProductsUserHasNotEnoughMoney_ThrowException()
        {
            Product product1 = _manager.AddProduct("milk");
            Product product2 = _manager.AddProduct("eggs");
            Product product3 = _manager.AddProduct("bread");
            Shop shop1 = _manager.AddShop("5");
            Shop shop2 = _manager.AddShop("FixPrice");
            
            _manager.DeliverGoodsInShop(shop1, product1, 10, 5);
            _manager.DeliverGoodsInShop(shop2, product1, 10, 100000);
            _manager.DeliverGoodsInShop(shop1, product2, 10, 2);
            _manager.DeliverGoodsInShop(shop1, product3, 10, 3);
            
            _customer.Shopping.AddProductOnShoppingList(product1, 20);
            _customer.Shopping.AddProductOnShoppingList(product2, 10);
            _customer.Shopping.AddProductOnShoppingList(product3, 10);
            _manager.CreatCart(_customer.Shopping);

            Assert.Catch<ShopsException>(() =>
            {
                _customer.Pay();
            });
        }
    }
}