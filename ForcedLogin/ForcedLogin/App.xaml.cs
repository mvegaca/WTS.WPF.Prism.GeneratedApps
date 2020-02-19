using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using ForcedLogin.Constants;
using ForcedLogin.Contracts.Services;
using ForcedLogin.Core.Contracts.Services;
using ForcedLogin.Core.Services;
using ForcedLogin.Models;
using ForcedLogin.Services;
using ForcedLogin.ViewModels;
using ForcedLogin.Views;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;

namespace ForcedLogin
{
    public partial class App : PrismApplication
    {
        private LogInWindow _logInWindow;

        private string[] _startUpArgs;

        public App()
        {
        }

        protected override Window CreateShell()
            => Container.Resolve<ShellWindow>();

        protected async override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);
            var persistAndRestoreService = Container.Resolve<IPersistAndRestoreService>();
            persistAndRestoreService.RestoreData();
            await Task.CompletedTask;
            var themeSelectorService = Container.Resolve<IThemeSelectorService>();
            themeSelectorService.SetTheme();
            var userDataService = Container.Resolve<IUserDataService>();
            userDataService.Initialize();
            var identityService = Container.Resolve<IIdentityService>();
            var config = Container.Resolve<AppConfig>();
            identityService.InitializeWithAadAndPersonalMsAccounts(config.IdentityClientId, "http://localhost");
            identityService.LoggedIn += OnLoggedIn;
            identityService.LoggedOut += OnLoggedOut;
            var silentLoginSuccess = await identityService.AcquireTokenSilentAsync();
            if (!silentLoginSuccess || !identityService.IsAuthorized())
            {
                ShowLogInWindow();
            }
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
            _logInWindow.Close();
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Hide();
            ShowLogInWindow();
        }

        private void ShowLogInWindow()
        {
            _logInWindow = Container.Resolve<LogInWindow>();
            _logInWindow.Closed += OnLogInWindowClosed;
            _logInWindow.ShowDialog();
        }

        private void OnLogInWindowClosed(object sender, EventArgs e)
        {
            if (sender is Window window)
            {
                window.Closed -= OnLogInWindowClosed;
                var identityService = Container.Resolve<IIdentityService>();
                if (!identityService.IsLoggedIn())
                {
                    Application.Current.Shutdown();
                }
            }
        }

        public async override void Initialize()
        {
            base.Initialize();
            await Task.CompletedTask;
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            _startUpArgs = e.Args;
            base.OnStartup(e);
            await Task.CompletedTask;
        }

        protected async override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Core Services
            containerRegistry.Register<IMicrosoftGraphService, MicrosoftGraphService>();

            PrismContainerExtension.Create(Container.GetContainer());
            PrismContainerExtension.Current.RegisterServices(s =>
            {
                s.AddHttpClient("msgraph", client =>
                {
                    client.BaseAddress = new System.Uri("https://graph.microsoft.com/v1.0/");
                });
            });

            containerRegistry.Register<IIdentityCacheService, IdentityCacheService>();
            containerRegistry.RegisterSingleton<IIdentityService, IdentityService>();
            containerRegistry.Register<IFileService, FileService>();

            // App Services
            containerRegistry.RegisterSingleton<IUserDataService, UserDataService>();
            containerRegistry.Register<ISystemService, SystemService>();
            containerRegistry.Register<IPersistAndRestoreService, PersistAndRestoreService>();
            containerRegistry.Register<IThemeSelectorService, ThemeSelectorService>();

            // Views
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>(PageKeys.Settings);
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>(PageKeys.Main);
            containerRegistry.RegisterForNavigation<ShellWindow, ShellViewModel>();

            // Configuration
            var configuration = BuildConfiguration();
            var appConfig = configuration
                .GetSection(nameof(AppConfig))
                .Get<AppConfig>();

            // Register configurations to IoC
            containerRegistry.RegisterInstance<IConfiguration>(configuration);
            containerRegistry.RegisterInstance<AppConfig>(appConfig);

            await Task.CompletedTask;
        }

        private IConfiguration BuildConfiguration()
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return new ConfigurationBuilder()
                .SetBasePath(appLocation)
                .AddJsonFile("appsettings.json")
                .AddCommandLine(_startUpArgs)
                .Build();
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            await Task.CompletedTask;
            var persistAndRestoreService = Container.Resolve<IPersistAndRestoreService>();
            persistAndRestoreService.PersistData();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/dotnet/api/system.windows.application.dispatcherunhandledexception?view=netcore-3.0

            // e.Handled = true;
        }
    }
}
