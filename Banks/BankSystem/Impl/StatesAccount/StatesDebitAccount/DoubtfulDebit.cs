using Banks.Tools.AccountException;

namespace Banks.BankSystem.Impl.StatesAccount.StatesDebitAccount
{
    internal class DoubtfulDebit : StateDebit
    {
        internal override void CheckWithdrawal(float money)
        {
            if (Account.Info.LimitDoubtfulAccount < money || Account.Money - money < 0 || money < 0)
            {
                throw new TransactionCannotBeMade();
            }
        }

        internal override void CheckReplenishment(float money)
        {
            if (Account.Info.LimitDoubtfulAccount < money || money < 0)
            {
                throw new TransactionCannotBeMade();
            }
        }

        internal override void Withdrawal(float money)
        {
            CheckWithdrawal(money);
            Account.Money -= money;
        }

        internal override void Replenishment(float money)
        {
            CheckReplenishment(money);
            Account.Money += money;
        }
    }
}