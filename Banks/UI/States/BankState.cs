using System;
using Banks.BankSystem.Impl;
using Spectre.Console;

namespace Banks.UI.States
{
    public class BankState : UiState
    {
        private Bank _bank;

        internal BankState(UiMain uiMain)
        {
            AnsiConsole.Markup("[blue]Bank name: [/]");
            _bank = CentralBank.GetInstance().GetBank(Console.ReadLine());
        }

        internal override void Start()
        {
            while (true)
            {
                string command = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter command")
                        .InvalidChoiceMessage("[red]Error: unknown command[/]")
                        .DefaultValue("help")
                        .AddChoice("addOffer")
                        .AddChoice("makeAccount")
                        .AddChoice("showOffers")
                        .AddChoice("showClients")
                        .AddChoice("logout"));
                switch (command)
                {
                    case "showClients":
                        UiAdapter.GetInstance().ShowClients(_bank);
                        break;
                    case "showOffers":
                        UiAdapter.GetInstance().ShowOffers(_bank);
                        break;
                    case "makeAccount":
                        UiAdapter.GetInstance().MakeAccount(_bank);
                        break;
                    case "addOffer":
                        UiAdapter.GetInstance().AddOffer(_bank);
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
            AnsiConsole.WriteLine("addOffer - add offer in the bank");
            AnsiConsole.WriteLine("makeAccount - create or found client with account in the bank");
            AnsiConsole.WriteLine("showOffers - show offers at the bank");
            AnsiConsole.WriteLine("showClients - show clients at the bank");
            AnsiConsole.WriteLine("logout - return in main menu");
        }
    }
}