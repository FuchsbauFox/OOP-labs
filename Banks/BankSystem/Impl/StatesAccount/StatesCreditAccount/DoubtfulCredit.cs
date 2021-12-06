using Banks.Tools.AccountException;

namespace Banks.BankSystem.Impl.StatesAccount.StatesCreditAccount
{
    internal class DoubtfulCredit : StateCredit
    {
        internal override void CheckWithdrawal(float money, bool withoutCommission = false)
        {
            if (!withoutCommission)
            {
                if (Account.Info.LimitDoubtfulAccount < money)
                    throw new TransactionCannotBeMade();

                if (Account.Money - money < 0)
                    money += Account.Info.Commission;
            }

            if (Account.Info.Limit < -1 * (Account.Money - money) || money < 0)
                throw new TransactionCannotBeMade();
        }

        internal override void CheckReplenishment(float money, bool withoutCommission = false)
        {
            if (!withoutCommission)
            {
                if (Account.Info.LimitDoubtfulAccount < money)
                    throw new TransactionCannotBeMade();

                if (Account.Money < 0)
                    money -= Account.Info.Commission;
            }

            if (money < 0)
                throw new TransactionCannotBeMade();
        }

        internal override void Withdrawal(float money, bool withoutCommission = false)
        {
            CheckWithdrawal(money, withoutCommission);
            if (Account.Money - money < 0 && !withoutCommission)
                money += Account.Info.Commission;
            Account.Money -= money;
        }

        internal override void Replenishment(float money, bool withoutCommission = false)
        {
            CheckReplenishment(money, withoutCommission);
            if (Account.Money < 0 && !withoutCommission)
                money -= Account.Info.Commission;
            Account.Money += money;
        }
    }
}