using HabitWorkbench.Services;

namespace HabitWorkbench
{
    public partial class App : Application
    {
        private readonly AppUpdateService _updates;
        private readonly IServiceProvider _services;

        public App(AppUpdateService updates, IServiceProvider services)
        {
            InitializeComponent();
            _updates = updates;
            _services = services;

            _updates.StartBackgroundCheck();

            //  Option A: init DB BEFORE Blazor loads
            MainPage = _services.GetRequiredService<StartupPage>();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Destroying += (_, __) =>
            {
                _updates.ApplyIfPendingRestart();
            };

            return window;
        }
    }
}
