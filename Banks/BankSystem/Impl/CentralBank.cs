using System.Collections.Generic;
using System.Linq;
using Banks.Tools.CentralBankException;

namespace Banks.BankSystem.Impl
{
    public class CentralBank
    {
        private static CentralBank _instance;
        private readonly List<Bank> _banks;

        private int _idLastBank;
        private CentralBank()
        {
            _banks = new List<Bank>();

            _idLastBank = 0;
        }

        public IReadOnlyList<Bank> Banks => _banks;

        public static CentralBank GetInstance()
        {
            return _instance ??= new CentralBank();
        }

        public void AddBank(string name)
        {
            CheckId();
            CheckBankOnExist(name);
            _banks.Add(new Bank(name, _idLastBank));
            _idLastBank++;
        }

        public Bank GetBank(string name)
        {
            CheckBankOnExist(name, true);
            return _banks.FirstOrDefault(bank => bank.Name == name);
        }

        private void CheckId()
        {
            if (_idLastBank > 999)
            {
                throw new BankCannotBeAddedException();
            }
        }

        private void CheckBankOnExist(string name, bool shouldExist = false)
        {
            bool bankExist = _banks.Any(bank => bank.Name == name);
            switch (shouldExist)
            {
                case false when bankExist:
                    throw new BankAlreadyExistException();
                case true when !bankExist:
                    throw new BankNotExistException();
            }
        }
    }
}