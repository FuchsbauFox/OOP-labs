using Banks.BankSystem.Impl;
using Banks.UI.States;

namespace Banks.UI
{
    public class UiMain
    {
        private static UiMain _instance;
        private UiState _uiState;

        private UiMain()
        {
            _uiState = new MainState();
            _uiState.SetContext(this);
        }

        public static UiMain GetInstance()
        {
            return _instance ??= new UiMain();
        }

        public void TransitionTo(UiState uiState)
        {
            _uiState = uiState;
            _uiState.SetContext(this);
        }

        public void Start()
        {
            _uiState.Start();
        }

        private void Help()
        {
            _uiState.Help();
        }
    }
}