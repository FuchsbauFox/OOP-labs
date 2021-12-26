namespace Banks.BankSystem.Impl.ClientBuilder
{
    public class Builder
    {
        private Client _client;

        public Builder()
        {
            Reset();
        }

        public void SetFullName(string fullName, string password)
        {
            _client.SetFullName(fullName, password);
        }

        public void SetPassport(string passport, string password)
        {
            _client.SetPassport(passport, password);
        }

        public void SetAddress(string address, string password)
        {
            _client.SetAddress(address, password);
        }

        public void SetPhoneNumber(string phoneNumber, string password)
        {
            _client.SetPhoneNumber(phoneNumber, password);
        }

        public Client GetClient()
        {
            Client client = _client;
            Reset();
            return client;
        }

        private void Reset()
        {
            _client = new Client();
        }
    }
}