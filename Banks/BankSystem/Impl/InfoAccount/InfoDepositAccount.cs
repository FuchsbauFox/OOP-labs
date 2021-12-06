using System;
using System.Collections.Generic;
using Banks.MyDateTime;
using Banks.Tools.InfoAccountException;

namespace Banks.BankSystem.Impl.InfoAccount
{
    public class InfoDepositAccount : IInfoAccount
    {
        private List<ItemInterest> _interests;

        public InfoDepositAccount(List<float> from, List<float> interest, float limitDoubtfulAccount)
        {
            CheckListInterests(from, interest);
            CheckLimitDoubtfulAccount(limitDoubtfulAccount);

            _interests = new List<ItemInterest>();
            for (int i = 0; i < from.Count; i++)
            {
                _interests.Add(new ItemInterest(from[i], interest[i]));
            }

            LimitDoubtfulAccount = limitDoubtfulAccount;
        }

        public IReadOnlyList<ItemInterest> Interests => _interests;
        public float LimitDoubtfulAccount { get; }

        public IInfoAccount DeepCopy()
        {
            var clone = (InfoDepositAccount)MemberwiseClone();
            clone._interests = new List<ItemInterest>(_interests);
            return clone;
        }

        private void CheckListInterests(List<float> from, List<float> interest)
        {
            if (from == null || interest == null)
            {
                throw new ArgumentNullException();
            }

            if (from.Count != interest.Count)
            {
                throw new ListsDontFitTogetherException();
            }

            if (from[0] != 0)
            {
                throw new FirstLimitDoesntStartAtZeroException();
            }

            if (interest[0] <= 0)
            {
                throw new NegativeOrNilInterestException();
            }

            for (int i = 1; i < from.Count; i++)
            {
                if (from[i - 1] >= from[i] || interest[i - 1] > interest[i])
                {
                    throw new ListsMustNotDecrease();
                }
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