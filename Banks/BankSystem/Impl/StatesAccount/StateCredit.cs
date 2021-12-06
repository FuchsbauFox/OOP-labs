namespace Banks.BankSystem.Impl.StatesAccount
{
    public abstract class StateCredit
    {
        protected CreditAccount Account { get; private set; }

        internal void SetContext(CreditAccount account)
        {
            Account = account;
        }

        internal abstract void CheckWithdrawal(float money, bool withoutCommission = false);
        internal abstract void CheckReplenishment(float money, bool withoutCommission = false);
        internal abstract void Withdrawal(float money, bool withoutCommission = false);
        internal abstract void Replenishment(float money, bool withoutCommission = false);
    }
}