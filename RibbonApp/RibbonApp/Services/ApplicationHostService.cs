using System;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Ioc;

using RibbonApp.Contracts.Services;
using RibbonApp.Contracts.Views;
using RibbonApp.ViewModels;

namespace RibbonApp.Services
{
    public class ApplicationHostService : IApplicationHostService
    {
        private readonly INavigationService _navigationService;
        private readonly IPersistAndRestoreService _persistAndRestoreService;
        private readonly IThemeSelectorService _themeSelectorService;
        private readonly IRightPaneService _rightPaneService;
        private IShellWindow _shellWindow;

        public ApplicationHostService(INavigationService navigationService, IRightPaneService rightPaneService, IThemeSelectorService themeSelectorService, IPersistAndRestoreService persistAndRestoreService)
        {
            _navigationService = navigationService;
            _rightPaneService = rightPaneService;
            _themeSelectorService = themeSelectorService;
            _persistAndRestoreService = persistAndRestoreService;
        }

        public async Task StartAsync()
        {
            // Initialize services that you need before app activation
            await InitializeAsync();

            _shellWindow = SimpleIoc.Default.GetInstance<IShellWindow>();
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
            _rightPaneService.Initialize(_shellWindow.GetRightPaneFrame(), _shellWindow.GetSplitView());
            _shellWindow.ShowWindow();
            _navigationService.NavigateTo(typeof(MainViewModel).FullName);

            // Tasks after activation
            await StartupAsync();
        }

        public async Task StopAsync()
        {
            await Task.CompletedTask;
            _persistAndRestoreService.PersistData();
        }

        private async Task InitializeAsync()
        {
            await Task.CompletedTask;
            _persistAndRestoreService.RestoreData();
            _themeSelectorService.SetTheme();
        }

        private async Task StartupAsync()
        {
            await Task.CompletedTask;
        }
    }
}
