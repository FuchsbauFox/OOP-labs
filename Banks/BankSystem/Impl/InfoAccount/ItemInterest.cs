using System.Collections.Generic;

namespace Banks.BankSystem.Impl.InfoAccount
{
    public class ItemInterest
    {
        public ItemInterest(float from, float interest)
        {
            From = from;
            Interest = interest;
        }

        public float From { get; }
        public float Interest { get; }
    }
}