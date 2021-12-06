using System;
using Banks.BankSystem;
using Banks.BankSystem.Impl;
using Banks.BankSystem.Impl.InfoAccount;
using Banks.UI;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            UiMain.GetInstance().Start();
        }
    }
}
