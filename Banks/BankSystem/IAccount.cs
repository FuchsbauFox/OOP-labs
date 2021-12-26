using System;
using System.Collections.Generic;
using Banks.BankSystem.Impl;

namespace Banks.BankSystem
{
    public interface IAccount
    {
        float Money { get; }
        string IdAccount { get; }
        DateTime OpenDate { get; }
        public IReadOnlyList<TransactionLog> Transactions();
        IInfoAccount GetInfo();
        void CheckWithdrawal(float money);
        void CheckReplenishment(float money);
        void Withdrawal(float money, string log = "Withdrawal");
        void Replenishment(float money, string log = "Replenishment");
        public void AccrualOfInterest();
        public void Block();
        public void Standard();
    }
}