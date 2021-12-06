using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Banks.BankSystem.Impl.InfoAccount;
using Banks.BankSystem.Impl.StatesAccount.StatesCreditAccount;
using Banks.BankSystem.Impl.StatesAccount.StatesDebitAccount;
using Banks.BankSystem.Impl.StatesAccount.StatesDepositAccount;
using Banks.MyDateTime;
using Banks.Tools.BankException;
using Banks.Tools.ClientException;

namespace Banks.BankSystem.Impl
{
    public class Bank
    {
        private readonly List<IInfoAccount> _offers;
        private readonly List<Client> _clients;
        private readonly List<Client> _observers;

        private int _idLastAccount;
        public Bank(string name, int id)
        {
            CheckName(name);
            Name = name;
            BankId = id;

            _idLastAccount = 0;
            _offers = new List<IInfoAccount>();
            _clients = new List<Client>();
            _observers = new List<Client>();
        }

        public string Name { get; }
        public int BankId { get; }
        public IReadOnlyList<IInfoAccount> Offers => _offers;
        public IReadOnlyList<Client> Clients => _clients;

        public void AddOffer(IInfoAccount info)
        {
            CheckOfferOnExist(info);

            _offers.Add(info);
            Notify(info);
        }

        public void ChangeOffer(IInfoAccount infoOffer, IInfoAccount newOffer)
        {
            CheckOfferOnExist(infoOffer, true);
            CheckOfferOnExist(newOffer);
            CheckInfoTypes(infoOffer, newOffer);

            _offers.Remove(infoOffer);
            _offers.Add(newOffer);
            Notify(newOffer);
        }

        public void MakeAccount(Client client, IInfoAccount info, string password, int monthsForDeposit = 0)
        {
            CheckOfferOnExist(info, true);
            CheckClient(client);
            CheckId();
            client.Login(password);
            CheckIfDeposit(info, monthsForDeposit);

            IAccount account = AccountCreator(
                info.DeepCopy(),
                client.Passport == null || client.Address == null,
                monthsForDeposit);

            client.AddAccount(account);
        }

        public void Attach(Client client)
        {
            if (client.PhoneNumber == null) throw new ClientCannotBeAttach();
            _observers.Add(client);
        }

        public void Detach(Client client)
        {
            _observers.Remove(client);
        }

        public Client FindClient(string fullName)
        {
            return _clients.FirstOrDefault(client => client.FullName == fullName);
        }

        private void Notify(IInfoAccount info)
        {
            foreach (Client observer in _observers)
            {
                observer.Update(info);
            }
        }

        private void CheckName(string name)
        {
            if (!Regex.IsMatch(name, @"[А-ЯЁа-яёA-Za-z]"))
            {
                throw new InvalidBankNameException();
            }
        }

        private IAccount AccountCreator(IInfoAccount info, bool isDoubtful, int monthsForDeposit = 0)
        {
            string id = BankId.ToString("000") + _idLastAccount.ToString("000000");
            switch (info)
            {
                case InfoCreditAccount _:
                {
                    var account = new CreditAccount(info.DeepCopy() as InfoCreditAccount, id);

                    if (isDoubtful) account.TransitionTo(new DoubtfulCredit());
                    else account.TransitionTo(new StandardCredit());

                    _idLastAccount++;
                    return account;
                }

                case InfoDebitAccount _:
                {
                    var account = new DebitAccount(info.DeepCopy() as InfoDebitAccount, id);

                    if (isDoubtful) account.TransitionTo(new DoubtfulDebit());
                    else account.TransitionTo(new StandardDebit());

                    _idLastAccount++;
                    return account;
                }

                case InfoDepositAccount _:
                {
                    var account = new DepositAccount(
                        info.DeepCopy() as InfoDepositAccount,
                        id,
                        CurrentDate.GetInstance().Date.AddMonths(monthsForDeposit));

                    if (isDoubtful) account.TransitionTo(new DoubtfulDeposit());
                    else account.TransitionTo(new StandardDeposit());

                    _idLastAccount++;
                    return account;
                }

                default:
                    throw new AccountCannotBeCreatedException();
            }
        }

        private void CheckOfferOnExist(IInfoAccount info, bool shouldExist = false)
        {
            bool offerExist = _offers.Any(offer => offer == info);
            switch (shouldExist)
            {
                case false when offerExist:
                    throw new OfferAlreadyExistException();
                case true when !offerExist:
                    throw new OfferNotExistException();
            }
        }

        private void CheckInfoTypes(IInfoAccount info1, IInfoAccount info2)
        {
            if (info1.GetType() != info2.GetType())
            {
                throw new TypesInfoAccountDifferentException();
            }
        }

        private void CheckClient(Client client)
        {
            if (client.Accounts.Count == 0)
            {
                _clients.Add(client);
                return;
            }

            if (_clients.All(bankClient => bankClient != client))
            {
                throw new ClientNotFoundException();
            }
        }

        private void CheckId()
        {
            if (_idLastAccount > 999999)
            {
                throw new AccountCannotBeCreatedException();
            }
        }

        private void CheckIfDeposit(IInfoAccount info, int months)
        {
            if (info is InfoDepositAccount && months == 0)
                throw new AccountCannotBeCreatedException();
        }
    }
}