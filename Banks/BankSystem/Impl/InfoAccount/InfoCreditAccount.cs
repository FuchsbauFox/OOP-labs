using Banks.Tools.InfoAccountException;

namespace Banks.BankSystem.Impl.InfoAccount
{
    public class InfoCreditAccount : IInfoAccount
    {
        public InfoCreditAccount(float commission, float limit, float limitDoubtfulAccount)
        {
            CheckCommission(commission, limitDoubtfulAccount);
            CheckLimit(limit);
            CheckLimitDoubtfulAccount(limitDoubtfulAccount);

            Commission = commission;
            Limit = limit;
            LimitDoubtfulAccount = limitDoubtfulAccount;
        }

        public float Commission { get; }
        public float Limit { get; }
        public float LimitDoubtfulAccount { get; }

        public IInfoAccount DeepCopy()
        {
            return (InfoCreditAccount)MemberwiseClone();
        }

        private void CheckCommission(float commission, float limitDoubtfulAccount)
        {
            if (commission <= 0)
            {
                throw new NegativeOrNilCommissionException();
            }

            if (commission > limitDoubtfulAccount)
            {
                throw new CommissionMoreThanLimitDoubtfulAccountException();
            }
        }

        private void CheckLimit(float limit)
        {
            if (limit <= 0)
            {
                throw new NegativeOrNilLimitException();
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