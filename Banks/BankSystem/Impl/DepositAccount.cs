using System;
using System.Collections.Generic;
using System.Linq;
using Banks.BankSystem.Impl.InfoAccount;
using Banks.BankSystem.Impl.StatesAccount;
using Banks.BankSystem.Impl.StatesAccount.StatesDepositAccount;
using Banks.MyDateTime;
using Banks.Tools.InfoAccountException;

namespace Banks.BankSystem.Impl
{
    public class DepositAccount : IAccount
    {
        private readonly List<TransactionLog> _transaction;
        private readonly Accrual _accrual;
        private StateDeposit _state;
        private int _idLastTransaction;

        internal DepositAccount(InfoDepositAccount info, string id, DateTime depositTime)
        {
            CheckDate(depositTime);

            _transaction = new List<TransactionLog>();
            _accrual = new Accrual(this, info.Interests.ToList());
            _state = null;
            _idLastTransaction = 0;
            Money = 0;
            Info = info;
            IdAccount = id;
            DepositEndDate = depositTime;
            OpenDate = CurrentDate.GetInstance().Date;
        }

        public float Money { get; internal set; }
        public InfoDepositAccount Info { get; }
        public string IdAccount { get; }
        public DateTime DepositEndDate { get; }
        public DateTime OpenDate { get; }
        public IReadOnlyList<TransactionLog> Transactions() => _transaction;

        public IInfoAccount GetInfo()
        {
            return Info;
        }

        public void TransitionTo(StateDeposit state)
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

            _transaction.Add(new TransactionLog(_idLastTransaction++, log, money));
        }

        public void Replenishment(float money, string log = "Replenishment")
        {
            _state.Replenishment(money);

            _transaction.Add(new TransactionLog(_idLastTransaction++, log, money));
        }

        public void AccrualOfInterest()
        {
            _accrual.NewDay();
        }

        public void Block()
        {
            TransitionTo(new BlockedDeposit());
        }

        public void Standard()
        {
            TransitionTo(new StandardDeposit());
        }

        private void CheckDate(DateTime date)
        {
            if (CurrentDate.GetInstance().Date > date)
            {
                throw new IncorrectDateForDepositException();
            }
        }
    }
}