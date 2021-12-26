namespace Banks.BankSystem.Impl.StatesAccount
{
    public abstract class StateDeposit
    {
        protected DepositAccount Account { get; private set; }

        internal void SetContext(DepositAccount account)
        {
            Account = account;
        }

        internal abstract void CheckWithdrawal(float money);
        internal abstract void CheckReplenishment(float money);
        internal abstract void Withdrawal(float money);
        internal abstract void Replenishment(float money);
    }
}