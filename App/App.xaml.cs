namespace Arcards
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

#if WINDOWS
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Width = 1080/2.5;
            window.Height = 2340/2.5;

            return window;
        }
#endif
    }
}
