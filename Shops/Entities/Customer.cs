using System.Linq;
using Shops.Tools;

namespace Shops.Entities
{
    public class Customer
    {
        private float _money;

        public Customer(float money = 1000)
        {
            Shopping = new Shopping();
            _money = money;
        }

        public Shopping Shopping { get; private set; }

        public void Pay()
        {
            float cost = Shopping.CalculateCost();
            CheckCostCart(cost);
            CheckForSolvency(cost);
            Shopping.MakeBuy();

            _money -= cost;
            Shopping = new Shopping();
        }

        private static void CheckCostCart(float cost)
        {
            if (cost == 0)
            {
                throw new CartIsEmptyException();
            }
        }

        private void CheckForSolvency(float cost)
        {
            if (cost > _money)
            {
                throw new CustomerCannotPayException();
            }
        }
    }
}