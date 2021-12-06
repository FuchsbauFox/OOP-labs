using Banks.Tools.AccountException;

namespace Banks.BankSystem.Impl.StatesAccount.StatesDebitAccount
{
    internal class BlockedDebit : StateDebit
    {
        internal override void CheckWithdrawal(float money)
        {
            throw new TransactionCannotBeMade();
        }

        internal override void CheckReplenishment(float money)
        {
            throw new TransactionCannotBeMade();
        }

        internal override void Withdrawal(float money)
        {
            CheckWithdrawal(money);
        }

        internal override void Replenishment(float money)
        {
            CheckReplenishment(money);
        }
    }
}