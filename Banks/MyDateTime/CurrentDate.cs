using System;
using System.Linq;
using Banks.BankSystem;
using Banks.BankSystem.Impl;

namespace Banks.MyDateTime
{
    public class CurrentDate
    {
        private static CurrentDate _instance;

        private CurrentDate()
        {
            Date = DateTime.Today;
        }

        public DateTime Date { get; private set; }

        public static CurrentDate GetInstance()
        {
            return _instance ??= new CurrentDate();
        }

        public void SkipDay()
        {
            Date = Date.AddDays(1);
            foreach (IAccount account in CentralBank.GetInstance().Banks.SelectMany(bank => bank.Clients)
                .SelectMany(client => client.Accounts))
            {
                account.AccrualOfInterest();
            }
        }

        public void SkipMonth()
        {
            Date = Date.AddMonths(1);
            foreach (IAccount account in CentralBank.GetInstance().Banks.SelectMany(bank => bank.Clients)
                .SelectMany(client => client.Accounts))
            {
                for (int i = 0; i < 30; i++)
                {
                    account.AccrualOfInterest();
                }
            }
        }

        public void SkipYear()
        {
            Date = Date.AddYears(1);
            foreach (IAccount account in CentralBank.GetInstance().Banks.SelectMany(bank => bank.Clients)
                .SelectMany(client => client.Accounts))
            {
                for (int i = 0; i < 365; i++)
                {
                    account.AccrualOfInterest();
                }
            }
        }
    }
}