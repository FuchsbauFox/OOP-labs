using System;
using Shops.Tools;

namespace Shops.Services
{
    internal class Variety
    {
        internal Variety(Product product, int quantity, int price)
        {
            CheckQuantity(quantity);
            CheckPrice(price);

            Id = product.Id;
            Quantity = quantity;
            QuantityTaken = 0;
            Price = price;
        }

        internal int Id { get; }
        internal int Quantity { get; private set; }
        internal int QuantityTaken { get; private set; }
        internal int Price { get; private set; }

        internal void Delivery(int deliveryQuantity)
        {
            CheckQuantity(deliveryQuantity);

            Quantity += deliveryQuantity;
        }

        internal void ChangePrice(int newPrice)
        {
            CheckPrice(newPrice);

            Price = newPrice;
        }

        internal void TakenProducts(int taken)
        {
            CheckQuantity(taken);
            CheckTakenPossible(taken);

            QuantityTaken = taken;
        }

        internal void Buy(int buy)
        {
            CheckBuyAndTake(buy);

            Quantity -= QuantityTaken;
            QuantityTaken = 0;
        }

        private static void CheckQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException();
            }
        }

        private static void CheckPrice(int price)
        {
            if (price <= 0)
            {
                throw new ArgumentException();
            }
        }

        private void CheckTakenPossible(int taken)
        {
            if (taken > Quantity)
            {
                throw new BuyNotPossibleException();
            }
        }

        private void CheckBuyAndTake(int buy)
        {
            if (buy != QuantityTaken)
            {
                throw new BuyNotPossibleException();
            }
        }
    }
}