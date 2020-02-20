using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;

using ForcedLogin.Constants;
using ForcedLogin.Contracts.Services;
using ForcedLogin.Core.Contracts.Services;
using ForcedLogin.Models;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ForcedLogin.ViewModels
{
    // TODO WTS: Change the URL for your privacy policy in the appsettings.json file, currently set to https://YourPrivacyUrlGoesHere
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly AppConfig _config;
        private readonly IUserDataService _userDataService;
        private readonly IIdentityService _identityService;
        private readonly IThemeSelectorService _themeSelectorService;
        private readonly IRegionManager _regionManager;
        private readonly IRegionNavigationService _navigationService;
        private readonly ISystemService _systemService;
        private AppTheme _theme;
        private string _versionDescription;
        private UserViewModel _user;
        private ICommand _setThemeCommand;
        private ICommand _privacyStatementCommand;
        private ICommand _logOutCommand;

        public AppTheme Theme
        {
            get { return _theme; }
            set { SetProperty(ref _theme, value); }
        }

        public string VersionDescription
        {
            get { return _versionDescription; }
            set { SetProperty(ref _versionDescription, value); }
        }

        public UserViewModel User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public ICommand SetThemeCommand => _setThemeCommand ?? (_setThemeCommand = new DelegateCommand<string>(OnSetTheme));

        public ICommand PrivacyStatementCommand => _privacyStatementCommand ?? (_privacyStatementCommand = new DelegateCommand(OnPrivacyStatement));

        public ICommand LogOutCommand => _logOutCommand ?? (_logOutCommand = new DelegateCommand(OnLogOut));

        public SettingsViewModel(AppConfig config, IThemeSelectorService themeSelectorService, ISystemService systemService, IUserDataService userDataService, IIdentityService identityService, IRegionManager regionManager)
        {
            _config = config;
            _themeSelectorService = themeSelectorService;
            _systemService = systemService;
            _userDataService = userDataService;
            _identityService = identityService;
            _regionManager = regionManager;
            _navigationService = _regionManager.Regions[Regions.Main].NavigationService;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            VersionDescription = GetVersionDescription();
            Theme = _themeSelectorService.GetCurrentTheme();
            _identityService.LoggedOut += OnLoggedOut;
            _userDataService.UserDataUpdated += OnUserDataUpdated;
            User = _userDataService.GetUser();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            UnregisterEvents();
        }

        private void UnregisterEvents()
        {
            _identityService.LoggedOut -= OnLoggedOut;
            _userDataService.UserDataUpdated -= OnUserDataUpdated;
        }

        private string GetVersionDescription()
        {
            var appName = "ForcedLogin";
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);
            return $"{appName} - {versionInfo.FileVersion}";
        }

        private void OnSetTheme(string themeName)
        {
            var theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
            _themeSelectorService.SetTheme(theme);
        }

        private void OnPrivacyStatement()
            => _systemService.OpenInWebBrowser(_config.PrivacyStatement);

        public bool IsNavigationTarget(NavigationContext navigationContext)
            => true;

        private async void OnLogOut()
        {
            await _identityService.LogoutAsync();
        }

        private void OnUserDataUpdated(object sender, UserViewModel userData)
        {
            User = userData;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            UnregisterEvents();
            _navigationService.RequestNavigate(PageKeys.Main);
            _navigationService.Journal.Clear();
        }
    }
}
