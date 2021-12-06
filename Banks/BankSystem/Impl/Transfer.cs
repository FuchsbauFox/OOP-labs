using System;
using System.Linq;
using System.Text.RegularExpressions;
using Banks.Tools.ClientException;

namespace Banks.BankSystem.Impl
{
    public class Transfer
    {
        private static Transfer _instance;

        private Transfer()
        {
        }

        public static Transfer GetInstance()
        {
            return _instance ??= new Transfer();
        }

        internal void MakeTransfer(string idSenderAccount, string idReceiverAccount, float money)
        {
            IAccount senderAccount = GetAccount(idSenderAccount);
            IAccount receiverAccount = GetAccount(idReceiverAccount);

            senderAccount.CheckWithdrawal(money);
            receiverAccount.CheckReplenishment(money);
            senderAccount.Withdrawal(money, "TransferTo: " + idReceiverAccount);
            receiverAccount.Replenishment(money, "TransferFrom: " + idSenderAccount);
        }

        internal void CancelTransfer(string idAccount, int idTransaction)
        {
            IAccount senderAccount = GetAccount(idAccount);

            TransactionLog commissionLog = senderAccount.Transactions().FirstOrDefault(transaction =>
                transaction.IdTransaction == idTransaction &&
                transaction.Type == "Commission");

            TransactionLog log = senderAccount.Transactions().FirstOrDefault(transaction =>
                transaction.IdTransaction == idTransaction &&
                Regex.IsMatch(transaction.Type, @"^TransferTo: [0-9]\d{8}"));

            if (log == null)
            {
                throw new TransactionNotFoundException();
            }

            IAccount receiverAccount = GetAccount(log.Type[(log.Type.IndexOf(" ", StringComparison.Ordinal) + 1) ..]);
            TransactionLog logReceiver = receiverAccount.Transactions().FirstOrDefault(transaction =>
                Math.Abs(transaction.Money - log.Money) < 0.001 && transaction.Type == "TransferFrom: " + idAccount);

            if (logReceiver == null)
            {
                throw new ErrorCancelTransaction();
            }

            receiverAccount.CheckWithdrawal(log.Money);
            senderAccount.CheckReplenishment(log.Money);
            receiverAccount.Withdrawal(log.Money, "CancelTransfer: " + logReceiver.IdTransaction);
            senderAccount.Replenishment(log.Money, "CancelTransfer: " + idTransaction);

            commissionLog?.CancelTransaction();
            logReceiver.CancelTransaction();
            log.CancelTransaction();
        }

        private IAccount GetAccount(string id)
        {
            IAccount findAccount = CentralBank.GetInstance().Banks[int.Parse(id[..3])].Clients
                .SelectMany(client => client.Accounts).FirstOrDefault(account => account.IdAccount == id);

            if (findAccount == null)
            {
                throw new AccountNotFoundException();
            }

            return findAccount;
        }
    }
}