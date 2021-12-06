using System;
using Banks.BankSystem.Impl;
using Banks.Tools.BankException;
using Spectre.Console;

namespace Banks.UI.States
{
    public class UserState : UiState
    {
        private Client _client;
        private string _password;

        internal UserState(UiMain uiMain)
        {
            AnsiConsole.Markup("[blue]Bank(name) to which the client belongs: [/]");
            Bank bank = CentralBank.GetInstance().GetBank(Console.ReadLine());
            AnsiConsole.Markup("[blue]Client full name: [/]");
            string fullName = Console.ReadLine();
            AnsiConsole.Markup("[blue]Client password: [/]");
            _password = Console.ReadLine();

            _client = bank.FindClient(fullName);
            if (_client == null)
            {
                throw new ClientNotFoundException();
            }

            _client.Login(_password);

            foreach (string notification in _client.Notifications)
            {
                AnsiConsole.MarkupLine("[yellow]" + notification + "[/]");
            }

            _client.ClearNotifications();
        }

        internal override void Start()
        {
            while (true)
            {
                string command = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter command")
                        .InvalidChoiceMessage("[red]Error: unknown command[/]")
                        .DefaultValue("help")
                        .AddChoice("fullName")
                        .AddChoice("passport")
                        .AddChoice("address")
                        .AddChoice("phone")
                        .AddChoice("showAccounts")
                        .AddChoice("withdrawal")
                        .AddChoice("replenishment")
                        .AddChoice("transfer")
                        .AddChoice("log")
                        .AddChoice("cancelTransfer")
                        .AddChoice("logout"));
                switch (command)
                {
                    case "help":
                        Help();
                        break;
                    case "fullName":
                        UiAdapter.GetInstance().NewFullName(_client, _password);
                        break;
                    case "passport":
                        UiAdapter.GetInstance().NewPassport(_client, _password);
                        break;
                    case "address":
                        UiAdapter.GetInstance().NewAddress(_client, _password);
                        break;
                    case "phone":
                        UiAdapter.GetInstance().NewPhoneNumber(_client, _password);
                        break;
                    case "showAccounts":
                        UiAdapter.GetInstance().ShowAccounts(_client);
                        break;
                    case "withdrawal":
                        UiAdapter.GetInstance().Withdrawal(_client);
                        break;
                    case "replenishment":
                        UiAdapter.GetInstance().Replenishment(_client);
                        break;
                    case "transfer":
                        UiAdapter.GetInstance().Transfer(_client);
                        break;
                    case "log":
                        UiAdapter.GetInstance().ShowLog(_client);
                        break;
                    case "cancelTransfer":
                        UiAdapter.GetInstance().CancelTransfer(_client);
                        break;
                    case "logout":
                        UiMain.TransitionTo(new MainState());
                        UiMain.Start();
                        break;
                }
            }
        }

        internal override void Help()
        {
            AnsiConsole.WriteLine("fullName - set new full name");
            AnsiConsole.WriteLine("passport - set new passport");
            AnsiConsole.WriteLine("address - set new address");
            AnsiConsole.WriteLine("phone - set new phone number");
            AnsiConsole.WriteLine("showAccounts - show client accounts");
            AnsiConsole.WriteLine("withdrawal - ATM withdrawal");
            AnsiConsole.WriteLine("replenishment - ATM replenishment");
            AnsiConsole.WriteLine("transfer - transfer money to another account");
            AnsiConsole.WriteLine("log - show log operations with account");
            AnsiConsole.WriteLine("cancelTransfer - cancel transfer money");
            AnsiConsole.WriteLine("logout- return in main menu");
        }
    }
}