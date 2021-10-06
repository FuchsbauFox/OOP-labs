using Shops.Services;
using Shops.Tools;

namespace Shops.Persons
{
    public class Customer
    {
        private int _money;

        public Customer(int money = 1000)
        {
            Shopping = new Shopping();
            _money = money;
        }

        public Shopping Shopping { get; private set; }

        public void Pay()
        {
            int cost = Shopping.CalculateCost();
            CheckCostCart(cost);
            CheckForSolvency(cost);
            Shopping.MakeBuy();

            _money -= cost;
            Shopping = new Shopping();
        }

        private void CheckCostCart(int cost)
        {
            if (cost == 0)
            {
                throw new CartIsEmptyException();
            }
        }

        private void CheckForSolvency(int cost)
        {
            if (cost > _money)
            {
                throw new CustomerCannotPayException();
            }
        }
    }
}