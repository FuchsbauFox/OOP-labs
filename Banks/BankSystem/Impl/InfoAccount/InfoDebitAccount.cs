using Banks.Tools.InfoAccountException;

namespace Banks.BankSystem.Impl.InfoAccount
{
    public class InfoDebitAccount : IInfoAccount
    {
        public InfoDebitAccount(float interestOnBalance, float limitDoubtfulAccount)
        {
            CheckInterest(interestOnBalance);
            CheckLimitDoubtfulAccount(limitDoubtfulAccount);

            InterestOnBalance = interestOnBalance;
            LimitDoubtfulAccount = limitDoubtfulAccount;
        }

        public float InterestOnBalance { get; }
        public float LimitDoubtfulAccount { get; }

        public IInfoAccount DeepCopy()
        {
            return (InfoDebitAccount)this.MemberwiseClone();
        }

        private void CheckInterest(float limit)
        {
            if (limit <= 0)
            {
                throw new NegativeOrNilInterestException();
            }
        }

        private void CheckLimitDoubtfulAccount(float limitDoubtfulAccount)
        {
            if (limitDoubtfulAccount <= 0)
            {
                throw new NegativeOrNilLimitDoubtfulAccountException();
            }
        }
    }
}