using System;
using System.Collections.Generic;
using Banks.BankSystem.Impl.InfoAccount;
using Banks.BankSystem.Impl.StatesAccount;
using Banks.BankSystem.Impl.StatesAccount.StatesDebitAccount;
using Banks.MyDateTime;

namespace Banks.BankSystem.Impl
{
    public class DebitAccount : IAccount
    {
        private readonly List<TransactionLog> _transactions;
        private readonly Accrual _accrual;
        private StateDebit _state;
        private int _idLastTransaction;

        internal DebitAccount(InfoDebitAccount info, string id)
        {
            _transactions = new List<TransactionLog>();
            _accrual = new Accrual(this, new List<ItemInterest> { new ItemInterest(0, info.InterestOnBalance) });
            _state = null;
            _idLastTransaction = 0;
            Money = 0;
            Info = info;
            IdAccount = id;
            OpenDate = CurrentDate.GetInstance().Date;
        }

        public float Money { get; internal set; }
        public InfoDebitAccount Info { get; }
        public string IdAccount { get; }
        public DateTime OpenDate { get; }
        public IReadOnlyList<TransactionLog> Transactions() => _transactions;

        public IInfoAccount GetInfo()
        {
            return Info;
        }

        public void TransitionTo(StateDebit state)
        {
            _state = state;

            _state.SetContext(this);
        }

        public void CheckWithdrawal(float money)
        {
            _state.CheckWithdrawal(money);
        }

        public void CheckReplenishment(float money)
        {
            _state.CheckReplenishment(money);
        }

        public void Withdrawal(float money, string log = "Withdrawal")
        {
            _state.Withdrawal(money);

            _transactions.Add(new TransactionLog(_idLastTransaction++, log, money));
        }

        public void Replenishment(float money, string log = "Replenishment")
        {
            _state.Replenishment(money);

            _transactions.Add(new TransactionLog(_idLastTransaction++, log, money));
        }

        public void AccrualOfInterest()
        {
            _accrual.NewDay();
        }

        public void Block()
        {
            TransitionTo(new BlockedDebit());
        }

        public void Standard()
        {
            TransitionTo(new StandardDebit());
        }
    }
}