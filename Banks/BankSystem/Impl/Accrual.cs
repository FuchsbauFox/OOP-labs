using System.Collections.Generic;
using System.Linq;
using Banks.BankSystem.Impl.InfoAccount;

namespace Banks.BankSystem.Impl
{
    internal class Accrual
    {
        private IAccount _account;
        private List<ItemInterest> _interests;

        public Accrual(IAccount account, List<ItemInterest> interests)
        {
            _account = account;
            _interests = new List<ItemInterest>(interests);
            DaysCounter = 0;
            Savings = 0;
        }

        public int DaysCounter { get; private set; }
        public float Savings { get; private set; }

        public void NewDay()
        {
            if (DaysCounter == 30)
            {
                _account.Replenishment(Savings, "Accrual");
                DaysCounter = 0;
                Savings = 0;
            }

            float interest = _interests.Last(item => item.From < _account.Money).Interest;
            Savings += _account.Money * interest / 365;
            DaysCounter++;
        }
    }
}