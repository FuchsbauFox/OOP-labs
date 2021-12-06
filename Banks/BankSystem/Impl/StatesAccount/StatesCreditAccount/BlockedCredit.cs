using Banks.Tools.AccountException;

namespace Banks.BankSystem.Impl.StatesAccount.StatesCreditAccount
{
    internal class BlockedCredit : StateCredit
    {
        internal override void CheckWithdrawal(float money, bool withoutCommission = false)
        {
            throw new TransactionCannotBeMade();
        }

        internal override void CheckReplenishment(float money, bool withoutCommission = false)
        {
            throw new TransactionCannotBeMade();
        }

        internal override void Withdrawal(float money, bool withoutCommission = false)
        {
            CheckWithdrawal(money);
        }

        internal override void Replenishment(float money, bool withoutCommission = false)
        {
            CheckReplenishment(money);
        }
    }
}