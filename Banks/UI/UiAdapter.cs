using System;
using System.Collections.Generic;
using System.Linq;
using Banks.BankSystem;
using Banks.BankSystem.Impl;
using Banks.BankSystem.Impl.ClientBuilder;
using Banks.BankSystem.Impl.InfoAccount;
using Banks.MyDateTime;
using Banks.Tools;
using Banks.Tools.BankException;
using Banks.Tools.UiException;
using Spectre.Console;

namespace Banks.UI
{
    internal class UiAdapter
    {
        private static UiAdapter _instance;

        private UiAdapter()
        {
        }

        internal static UiAdapter GetInstance()
        {
            return _instance ??= new UiAdapter();
        }

        internal void ShowBanks()
        {
            foreach (Bank bank in CentralBank.GetInstance().Banks)
            {
                AnsiConsole.WriteLine("Id: " + bank.BankId + "\t" + "Bank: " + bank.Name);
            }
        }

        internal void AddBank()
        {
            try
            {
                AnsiConsole.Markup("[blue]Bank name: [/]");
                string name = Console.ReadLine();
                if (name == "\n")
                    throw new ArgumentException();
                CentralBank.GetInstance().AddBank(name);
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        internal void SkipDay()
        {
            CurrentDate.GetInstance().SkipDay();
        }

        internal void SkipMonth()
        {
            CurrentDate.GetInstance().SkipMonth();
        }

        internal void SkipYear()
        {
            CurrentDate.GetInstance().SkipYear();
        }

        internal void ShowClients(Bank bank)
        {
            int counter = 1;
            foreach (Client client in bank.Clients)
            {
                AnsiConsole.MarkupLine("[underline yellow]==== " + counter + " ====[/]");
                counter++;
                AnsiConsole.WriteLine("FullName: " + client.FullName);
                AnsiConsole.WriteLine("Passport: " + client.Passport);
                AnsiConsole.WriteLine("Address: " + client.Address);
                AnsiConsole.WriteLine("Phone number: " + client.PhoneNumber);
                ShowAccounts(client);
            }
        }

        internal void ShowOffers(Bank bank)
        {
            int counter = 1;
            foreach (IInfoAccount offer in bank.Offers)
            {
                AnsiConsole.MarkupLine("[underline yellow]==== " + counter + " ====[/]");
                counter++;
                OfferOutput(offer);
            }
        }

        internal void MakeAccount(Bank bank)
        {
            try
            {
                int numberOfMonths = 0;
                AnsiConsole.Markup("[underline blue]Client fullname: [/]");
                string fullName = Console.ReadLine();
                AnsiConsole.Markup("[underline blue]Client password: [/]");
                string password = Console.ReadLine();
                IInfoAccount info = GetOffer(bank);
                Client client = bank.FindClient(fullName) ?? CreateClient(fullName, password);

                if (info is InfoDepositAccount)
                {
                    AnsiConsole.MarkupLine("[underline yellow]Current date: " + CurrentDate.GetInstance().Date + "[/]");
                    AnsiConsole.Markup("[blue]Number of months for which the deposit is provided: [/]");
                    if (!int.TryParse(Console.ReadLine(), out numberOfMonths))
                        throw new ArgumentException();
                }

                bank.MakeAccount(client, info, password, numberOfMonths);
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        internal void AddOffer(Bank bank)
        {
            try
            {
                string type = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter type offer")
                        .InvalidChoiceMessage("[red]Error: unknown type[/]")
                        .AddChoice("credit")
                        .AddChoice("debit")
                        .AddChoice("deposit"));
                switch (type)
                {
                    case "credit":
                    {
                        AnsiConsole.Markup("[blue]Enter commission: [/]");
                        if (!float.TryParse(Console.ReadLine(), out float commission))
                            throw new ArgumentException();
                        AnsiConsole.Markup("[blue]Enter limit: [/]");
                        if (!float.TryParse(Console.ReadLine(), out float limit))
                            throw new ArgumentException();
                        AnsiConsole.Markup("[blue]Enter limit doubtful account: [/]");
                        if (!float.TryParse(Console.ReadLine(), out float limitDoubtfulAccount))
                            throw new ArgumentException();

                        bank.AddOffer(new InfoCreditAccount(commission, limit, limitDoubtfulAccount));
                        break;
                    }

                    case "debit":
                    {
                        AnsiConsole.Markup("[blue]Enter interest on balance: [/]");
                        if (!float.TryParse(Console.ReadLine(), out float interestOnBalance))
                            throw new ArgumentException();
                        AnsiConsole.Markup("[blue]Enter limit doubtful account: [/]");
                        if (!float.TryParse(Console.ReadLine(), out float limitDoubtfulAccount))
                            throw new ArgumentException();

                        bank.AddOffer(new InfoDebitAccount(interestOnBalance, limitDoubtfulAccount));
                        break;
                    }

                    case "deposit":
                    {
                        AnsiConsole.MarkupLine("[blue]Enter interest on balance: [/]");
                        var fromList = new List<float>();
                        var interestList = new List<float>();
                        while (true)
                        {
                            AnsiConsole.Markup("From: ");
                            if (!float.TryParse(Console.ReadLine(), out float from))
                                throw new ArgumentException();
                            fromList.Add(from);
                            AnsiConsole.Markup("Interest: ");
                            if (!float.TryParse(Console.ReadLine(), out float interest))
                                throw new ArgumentException();
                            interestList.Add(interest);

                            if (!AnsiConsole.Confirm("[underline yellow]Continue enter?[/]"))
                            {
                                break;
                            }
                        }

                        AnsiConsole.Markup("[blue]Enter limit doubtful account: [/]");
                        if (!float.TryParse(Console.ReadLine(), out float limitDoubtfulAccount))
                            throw new ArgumentException();

                        bank.AddOffer(new InfoDepositAccount(
                            fromList,
                            interestList,
                            limitDoubtfulAccount));
                        break;
                    }

                    default:
                        AnsiConsole.MarkupLine("[underline red]Error: account type unknown[/]");
                        break;
                }
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        internal void Attach(Bank bank)
        {
            try
            {
                AnsiConsole.Markup("[underline blue]Client fullname: [/]");
                string fullName = Console.ReadLine();
                AnsiConsole.Markup("[underline blue]Client password: [/]");
                string password = Console.ReadLine();
                Client client = bank.FindClient(fullName);
                if (client == null)
                    throw new ClientNotFoundException();
                client.Login(password);
                bank.Attach(client);
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        internal void Detach(Bank bank)
        {
            try
            {
                AnsiConsole.Markup("[underline blue]Client fullname: [/]");
                string fullName = Console.ReadLine();
                AnsiConsole.Markup("[underline blue]Client password: [/]");
                string password = Console.ReadLine();
                Client client = bank.FindClient(fullName);
                if (client == null)
                    throw new ClientNotFoundException();
                client.Login(password);
                bank.Detach(client);
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        internal void ShowAccounts(Client client)
        {
            AnsiConsole.MarkupLine("[green]Accounts:[/]");
            int counter = 1;
            foreach (IAccount account in client.Accounts)
            {
                AnsiConsole.MarkupLine("[green]==== " + counter + " ====[/]");
                counter++;

                AnsiConsole.MarkupLine("Id: " + account.IdAccount);
                AnsiConsole.MarkupLine("Money: " + account.Money);
                if (account.GetInfo() is InfoDepositAccount)
                    AnsiConsole.MarkupLine("[green]Date deposit: [/]" + ((DepositAccount)account).DepositEndDate);
                OfferOutput(account.GetInfo());
                AnsiConsole.MarkupLine("[green]Date created: [/]" + account.OpenDate);
            }
        }

        internal void NewFullName(Client client, string password)
        {
            try
            {
                AnsiConsole.Markup("[blue]New full name: [/]");
                client.SetFullName(Console.ReadLine(), password);
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        internal void NewPassport(Client client, string password)
        {
            try
            {
                AnsiConsole.Markup("[blue]New passport: [/]");
                client.SetPassport(Console.ReadLine(), password);
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        internal void NewAddress(Client client, string password)
        {
            try
            {
                AnsiConsole.Markup("[blue]New address: [/]");
                client.SetAddress(Console.ReadLine(), password);
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        internal void NewPhoneNumber(Client client, string password)
        {
            try
            {
                AnsiConsole.Markup("[blue]New phone number: [/]");
                client.SetPhoneNumber(Console.ReadLine(), password);
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        internal void Withdrawal(Client client)
        {
            try
            {
                AnsiConsole.Markup("[blue]Enter account id [green]FROM[/] which you want to withdraw money: [/]");
                string idMyAccount = Console.ReadLine();
                AnsiConsole.Markup("[blue]Money: [/]");
                if (!float.TryParse(Console.ReadLine(), out float money))
                    throw new ArgumentException();
                client.Withrowal(idMyAccount, money);
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        internal void Replenishment(Client client)
        {
            try
            {
                AnsiConsole.Markup("[blue]Enter account id [green]TO[/] which you want to replenishment money: [/]");
                string idMyAccount = Console.ReadLine();
                AnsiConsole.Markup("[blue]Money: [/]");
                if (!float.TryParse(Console.ReadLine(), out float money))
                    throw new ArgumentException();
                client.Replenishment(idMyAccount, money);
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        internal void Transfer(Client client)
        {
            try
            {
                AnsiConsole.Markup("[blue]Enter account id [green]FROM[/] which you want to transfer money: [/]");
                string idMyAccount = Console.ReadLine();
                AnsiConsole.Markup("[blue]Enter account id [red]TO[/] which you want to transfer money: [/]");
                string idReceiverAccount = Console.ReadLine();
                AnsiConsole.Markup("[blue]Money: [/]");
                if (!float.TryParse(Console.ReadLine(), out float money))
                    throw new ArgumentException();
                client.MakeTransfer(idMyAccount, idReceiverAccount, money);
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        internal void ShowLog(Client client)
        {
            try
            {
                AnsiConsole.Markup("[blue]Enter account id: [/]");
                string idMyAccount = Console.ReadLine();

                foreach (TransactionLog log in client.Accounts.First(account => account.IdAccount == idMyAccount)
                    .Transactions())
                {
                    AnsiConsole.Markup(
                        log.IdTransaction + ":\t[yellow]" + log.Type + "[/]\t[green]" + log.Money + "[/]\n");
                }
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        internal void CancelTransfer(Client client)
        {
            try
            {
                AnsiConsole.Markup("[blue]Enter account id: [/]");
                string idMyAccount = Console.ReadLine();
                AnsiConsole.Markup("[blue]Enter id transaction: [/]");
                if (!int.TryParse(Console.ReadLine(), out int id))
                    throw new ArgumentException();
                client.CancelTransfer(idMyAccount, id);
            }
            catch (Exception exception)
            {
                AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
            }
        }

        private Client CreateClient(string fullName, string password)
        {
            var builder = new Builder();
            var director = new Director
            {
                Builder = builder,
            };
            AnsiConsole.MarkupLine("[underline yellow]If you do not want to enter data, then enter -[/]");

            AnsiConsole.Markup("[blue]Client passport: [/]");
            string passport = Console.ReadLine();
            if (passport == "-")
                passport = null;

            AnsiConsole.Markup("[blue]Client address: [/]");
            string address = Console.ReadLine();
            if (address == "-")
                address = null;

            AnsiConsole.Markup("[blue]Client phone number: [/]");
            string phoneNumber = Console.ReadLine();
            if (phoneNumber == "-")
                phoneNumber = null;

            director.BuildClient(fullName, password, passport, address, phoneNumber);
            return builder.GetClient();
        }

        private IInfoAccount GetOffer(Bank bank)
        {
            AnsiConsole.Markup("[blue]Enter the offer number: [/]");
            string str = Console.ReadLine();
            bool success = int.TryParse(str, out int number);
            if (!success || number > bank.Offers.Count) throw new UnknownCommand();
            return bank.Offers.ToList()[number - 1];
        }

        private void OfferOutput(IInfoAccount offer)
        {
            switch (offer)
            {
                case InfoCreditAccount account1:
                    AnsiConsole.MarkupLine("[green]Offer credit account: [/]");
                    AnsiConsole.WriteLine("Commission: " + account1.Commission);
                    AnsiConsole.WriteLine("Limit: " + account1.Limit);
                    AnsiConsole.WriteLine("Limit doubtful account: " + account1.LimitDoubtfulAccount);
                    break;
                case InfoDebitAccount account2:
                    AnsiConsole.MarkupLine("[green]Offer debit account: [/]");
                    AnsiConsole.WriteLine("Interest on balance: " + account2.InterestOnBalance);
                    AnsiConsole.WriteLine("Limit doubtful account: " + account2.LimitDoubtfulAccount);
                    break;
                case InfoDepositAccount account3:
                    AnsiConsole.MarkupLine("[green]Offer deposit account: [/]");
                    AnsiConsole.WriteLine("Interest on balance: ");
                    foreach (ItemInterest item in account3.Interests)
                        AnsiConsole.WriteLine("From: " + item.From + "\tinterest: " + item.Interest);
                    AnsiConsole.WriteLine("Limit doubtful account: " + account3.LimitDoubtfulAccount);
                    break;
            }
        }
    }
}