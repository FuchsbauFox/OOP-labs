using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Banks.BankSystem.Impl.InfoAccount;
using Banks.BankSystem.Impl.StatesAccount;
using Banks.BankSystem.Impl.StatesAccount.StatesCreditAccount;
using Banks.MyDateTime;

namespace Banks.BankSystem.Impl
{
    public class CreditAccount : IAccount
    {
        private readonly List<TransactionLog> _transaction;
        private StateCredit _state;
        private int _idLastTransaction;

        internal CreditAccount(InfoCreditAccount info, string id)
        {
            _transaction = new List<TransactionLog>();
            _state = null;
            _idLastTransaction = 0;
            Money = 0;
            Info = info;
            IdAccount = id;
            OpenDate = CurrentDate.GetInstance().Date;
        }

        public float Money { get; internal set; }
        public InfoCreditAccount Info { get; }
        public string IdAccount { get; }
        public DateTime OpenDate { get; }
        public IReadOnlyList<TransactionLog> Transactions() => _transaction;

        public IInfoAccount GetInfo()
        {
            return Info;
        }

        public void TransitionTo(StateCredit state)
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
            if (Money - money < 0 && Regex.IsMatch(log, @"CancelTransfer: [0-9]+"))
            {
                _state.Withdrawal(money - Info.Commission, true);
            }
            else
            {
                if (Money - money < 0)
                    _transaction.Add(new TransactionLog(_idLastTransaction, "Commission", Info.Commission));
                _state.Withdrawal(money);
            }

            _transaction.Add(new TransactionLog(_idLastTransaction++, log, money));
        }

        public void Replenishment(float money, string log = "Replenishment")
        {
            if (Money < 0 && Regex.IsMatch(log, @"CancelTransfer: [0-9]+"))
            {
                _state.Replenishment(money + Info.Commission, true);
            }
            else
            {
                if (Money < 0)
                    _transaction.Add(new TransactionLog(_idLastTransaction, "Commission", Info.Commission));
                _state.Replenishment(money);
            }

            _transaction.Add(new TransactionLog(_idLastTransaction++, log, money));
        }

        public void AccrualOfInterest()
        {
        }

        public void Block()
        {
            TransitionTo(new BlockedCredit());
        }

        public void Standard()
        {
            TransitionTo(new StandardCredit());
        }
    }
}