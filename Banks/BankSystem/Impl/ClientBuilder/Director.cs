namespace Banks.BankSystem.Impl.ClientBuilder
{
    public class Director
    {
        private Builder _builder;

        public Builder Builder
        {
            set => _builder = value;
        }

        public void BuildClient(
            string fullName,
            string password,
            string passport = null,
            string address = null,
            string phoneNumber = null)
        {
            _builder.SetFullName(fullName, password);
            if (passport != null)
                _builder.SetPassport(passport, password);
            if (address != null)
                _builder.SetAddress(address, password);
            if (phoneNumber != null)
                _builder.SetPhoneNumber(phoneNumber, password);
        }
    }
}