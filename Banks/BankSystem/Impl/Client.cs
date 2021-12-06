using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Banks.BankSystem.Impl.InfoAccount;
using Banks.Tools.ClientException;

namespace Banks.BankSystem.Impl
{
    public class Client
    {
        private List<IAccount> _accounts;
        private string _password;

        public Client()
        {
            _accounts = new List<IAccount>();
            _password = null;
            FullName = null;
            Passport = null;
            Address = null;
            PhoneNumber = null;
            Notifications = new List<string>();
        }

        public string FullName { get; private set; }
        public string Passport { get; private set; }
        public string Address { get; private set; }
        public string PhoneNumber { get; private set; }
        public List<string> Notifications { get; }
        public IReadOnlyList<IAccount> Accounts => _accounts;

        public void Withrowal(string idMyAccount, float money)
        {
            CheckAccount(idMyAccount);
            _accounts.First(account => account.IdAccount == idMyAccount).Withdrawal(money);
        }

        public void Replenishment(string idMyAccount, float money)
        {
            CheckAccount(idMyAccount);
            _accounts.First(account => account.IdAccount == idMyAccount).Replenishment(money);
        }

        public void MakeTransfer(string idMyAccount, string idReceiverAccount, float money)
        {
            CheckAccount(idMyAccount);
            Transfer.GetInstance().MakeTransfer(idMyAccount, idReceiverAccount, money);
        }

        public void CancelTransfer(string idMyAccount, int idTransaction)
        {
            CheckAccount(idMyAccount);
            Transfer.GetInstance().CancelTransfer(idMyAccount, idTransaction);
        }

        public void BlockAccount(string idAccount)
        {
            CheckAccount(idAccount);
            _accounts.First(myAccount => myAccount.IdAccount == idAccount).Block();
        }

        public void Update(IInfoAccount info)
        {
            switch (info)
            {
                case InfoCreditAccount _ when _accounts.Any(account => account is CreditAccount):
                    Notifications.Add("There is a new credit offer");
                    break;
                case InfoDebitAccount _ when _accounts.Any(account => account is DebitAccount):
                    Notifications.Add("There is a new debit offer");
                    break;
                case InfoDepositAccount _ when _accounts.Any(account => account is DepositAccount):
                    Notifications.Add("There is a new deposit offer");
                    break;
            }
        }

        public void SetFullName(string fullName, string password)
        {
            CheckPassword(password);
            CheckName(fullName);
            if (_password == null)
            {
                _password = password;
            }
            else if (_password != password)
            {
                throw new IncorrectPassword();
            }

            FullName = fullName;
        }

        public void SetPassport(string passport, string password)
        {
            CheckPassword(password);
            CheckPassport(passport);

            if (_password != password)
            {
                throw new IncorrectPassword();
            }

            Passport = passport;
            if (Passport == null || Address == null) return;

            foreach (IAccount account in _accounts)
            {
                account.Standard();
            }
        }

        public void SetAddress(string address, string password)
        {
            CheckPassword(password);
            CheckAddress(address);

            if (_password != password)
            {
                throw new IncorrectPassword();
            }

            Address = address;
            if (Passport == null || Address == null) return;

            foreach (IAccount account in _accounts)
            {
                account.Standard();
            }
        }

        public void SetPhoneNumber(string phoneNumber, string password)
        {
            CheckPassword(password);
            CheckPhoneNumber(phoneNumber);

            if (_password != password) throw new IncorrectPassword();

            PhoneNumber = phoneNumber;
        }

        internal void Login(string password)
        {
            if (password != _password) throw new IncorrectPassword();
        }

        internal void AddAccount(IAccount account)
        {
            _accounts.Add(account);
        }

        private void CheckAccount(string id)
        {
            if (_accounts.All(account => account.IdAccount != id))
            {
                throw new AccountNotFoundException();
            }
        }

        private void CheckPassword(string password)
        {
            if (Regex.IsMatch(password, @"[\/\\\:\*\?\<\>\|]"))
            {
                throw new InvalidPassword();
            }
        }

        private void CheckName(string name)
        {
            if (!Regex.IsMatch(name, @"^(?=.{1,41}$)[А-ЯЁ]\d{0}[а-яё]+(?:[-' ][А-ЯЁ]\d{0}[а-яё]+)$"))
            {
                throw new InvalidClientNameException();
            }
        }

        private void CheckPassport(string passport)
        {
            if (!Regex.IsMatch(passport, @"[0-9]\d{1} [0-9]\d{1} [0-9]\d{5}"))
            {
                throw new InvalidPassportException();
            }
        }

        private void CheckAddress(string address)
        {
            if (address == null)
            {
                throw new ArgumentNullException();
            }
        }

        private void CheckPhoneNumber(string phoneNumber)
        {
            if (!Regex.IsMatch(phoneNumber, @"\+7[0-9]\d{9}"))
            {
                throw new InvalidPassportException();
            }
        }
    }
}