namespace Banks.UI
{
    public abstract class UiState
    {
        protected UiMain UiMain { get; set; }

        internal void SetContext(UiMain ui)
        {
            UiMain = ui;
        }

        internal abstract void Start();
        internal abstract void Help();
    }
}