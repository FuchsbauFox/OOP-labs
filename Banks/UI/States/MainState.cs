using System;
using Spectre.Console;

namespace Banks.UI.States
{
    public class MainState : UiState
    {
        internal override void Start()
        {
            while (true)
            {
                string command = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter command")
                        .InvalidChoiceMessage("[red]Error: unknown command[/]")
                        .DefaultValue("help")
                        .AddChoice("addBank")
                        .AddChoice("showBanks")
                        .AddChoice("manager")
                        .AddChoice("user")
                        .AddChoice("exit"));
                switch (command)
                {
                    case "help":
                        Help();
                        break;
                    case "manager":
                        try
                        {
                            UiMain.TransitionTo(new BankState(UiMain));
                            UiMain.Start();
                        }
                        catch (Exception exception)
                        {
                            AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
                            UiMain.TransitionTo(new MainState());
                        }

                        break;
                    case "user":
                        try
                        {
                            UiMain.TransitionTo(new UserState(UiMain));
                            UiMain.Start();
                        }
                        catch (Exception exception)
                        {
                            AnsiConsole.MarkupLine("[underline red]Error: " + exception.Message + "[/]");
                            UiMain.TransitionTo(new MainState());
                        }

                        break;
                    case "showBanks":
                        UiAdapter.GetInstance().ShowBanks();
                        break;
                    case "addBank":
                        UiAdapter.GetInstance().AddBank();
                        break;
                    case "exit":
                        return;
                }
            }
        }

        internal override void Help()
        {
            AnsiConsole.WriteLine("addBank - add bank");
            AnsiConsole.WriteLine("showBanks - show registered banks");
            AnsiConsole.WriteLine("manager - login as manager");
            AnsiConsole.WriteLine("user - login as user");
            AnsiConsole.WriteLine("exit - finish work");
        }
    }
}