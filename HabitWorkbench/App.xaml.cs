using HabitWorkbench.Services;

namespace HabitWorkbench
{
    public partial class App : Application
    {
        private readonly AppUpdateService _updates;

        public App(AppUpdateService updates)
        {
            InitializeComponent();
            _updates = updates;

            // Start silent check/download early
            _updates.StartBackgroundCheck();

            MainPage = new MainPage();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            // Apply update when the app/window is closing
            window.Destroying += (_, __) =>
            {
                _updates.ApplyIfPendingRestart();
            };

            return window;
        }
    }
}
