using System;
using System.Windows;
using System.Windows.Input;

using MenuBar.Constants;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace MenuBar.ViewModels
{
    public class ShellViewModel : BindableBase, IDisposable
    {
        private readonly IRegionManager _regionManager;
        private IRegionNavigationService _navigationService;
        private IRegionNavigationService _rightPanenavigationService;
        private DelegateCommand _goBackCommand;
        private ICommand _menuFileSettingsCommand;

        private ICommand _menuViewsWebViewCommand;

        private ICommand _menuViewsMasterDetailCommand;

        private ICommand _menuViewsMainCommand;

        private ICommand _loadedCommand;
        private ICommand _menuFileExitCommand;

        public DelegateCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack, CanGoBack));

        public ICommand MenuFileSettingsCommand => _menuFileSettingsCommand ?? (_menuFileSettingsCommand = new DelegateCommand(OnMenuFileSettings));

        public ICommand MenuViewsWebViewCommand => _menuViewsWebViewCommand ?? (_menuViewsWebViewCommand = new DelegateCommand(OnMenuViewsWebView));

        public ICommand MenuViewsMasterDetailCommand => _menuViewsMasterDetailCommand ?? (_menuViewsMasterDetailCommand = new DelegateCommand(OnMenuViewsMasterDetail));

        public ICommand MenuViewsMainCommand => _menuViewsMainCommand ?? (_menuViewsMainCommand = new DelegateCommand(OnMenuViewsMain));

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new DelegateCommand(OnLoaded));

        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new DelegateCommand(OnMenuFileExit));

        public ShellViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private void OnLoaded()
        {
            _navigationService = _regionManager.Regions[Regions.Main].NavigationService;
            _rightPanenavigationService = _regionManager.Regions[Regions.RightPane].NavigationService;
            _navigationService.Navigated += OnNavigated;
        }

        public void Dispose()
        {
            _navigationService.Navigated -= OnNavigated;
        }

        private bool CanGoBack()
            => _navigationService != null && _navigationService.Journal.CanGoBack;

        private void OnGoBack()
            => _navigationService.Journal.GoBack();

        private bool RequestNavigate(string target)
        {
            if (_navigationService.CanNavigate(target))
            {
                _navigationService.RequestNavigate(target);
                return true;
            }

            return false;
        }

        private bool RequestNavigateOnRightPane(string target)
        {
            if (_rightPanenavigationService.CanNavigate(target))
            {
                _rightPanenavigationService.RequestNavigate(target);
                return true;
            }

            return false;
        }

        private void RequestNavigateAndCleanJournal(string target)
        {
            var navigated = RequestNavigate(target);
            if (navigated)
            {
                _navigationService.Journal.Clear();
            }
        }

        private void OnNavigated(object sender, RegionNavigationEventArgs e)
            => GoBackCommand.RaiseCanExecuteChanged();

        private void OnMenuFileExit()
            => Application.Current.Shutdown();

        private void OnMenuViewsMain()
            => RequestNavigateAndCleanJournal(PageKeys.Main);

        private void OnMenuViewsMasterDetail()
            => RequestNavigateAndCleanJournal(PageKeys.MasterDetail);

        private void OnMenuViewsWebView()
            => RequestNavigateAndCleanJournal(PageKeys.WebView);

        private void OnMenuFileSettings()
            => RequestNavigateOnRightPane(PageKeys.Settings);
    }
}
