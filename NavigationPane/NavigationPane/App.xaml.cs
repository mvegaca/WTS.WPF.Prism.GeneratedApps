using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Extensions.Configuration;

using NavigationPane.Constants;
using NavigationPane.Contracts.Services;
using NavigationPane.Core.Contracts.Services;
using NavigationPane.Core.Services;
using NavigationPane.Models;
using NavigationPane.Services;
using NavigationPane.ViewModels;
using NavigationPane.Views;

using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;

namespace NavigationPane
{
    public partial class App : PrismApplication
    {
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
            containerRegistry.Register<IFileService, FileService>();

            // App Services
            containerRegistry.Register<IPersistAndRestoreService, PersistAndRestoreService>();
            containerRegistry.Register<IThemeSelectorService, ThemeSelectorService>();
            containerRegistry.Register<ISystemService, SystemService>();
            containerRegistry.Register<ISampleDataService, SampleDataService>();

            // Views
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>(PageKeys.Settings);
            containerRegistry.RegisterForNavigation<WebViewPage, WebViewViewModel>(PageKeys.WebView);
            containerRegistry.RegisterForNavigation<MasterDetailPage, MasterDetailViewModel>(PageKeys.MasterDetail);
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
