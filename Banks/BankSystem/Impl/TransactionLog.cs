namespace Banks.BankSystem.Impl
{
    public class TransactionLog
    {
        public TransactionLog(int idTransaction, string type, float money)
        {
            IdTransaction = idTransaction;
            Type = type;
            Money = money;
        }

        public int IdTransaction { get; }
        public string Type { get; private set; }
        public float Money { get; }

        internal void CancelTransaction()
        {
            Type = "Transaction was cancel";
        }
    }
}