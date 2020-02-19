using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using RibbonApp.Contracts.Services;
using RibbonApp.Contracts.ViewModels;
using RibbonApp.Models;

namespace RibbonApp.ViewModels
{
    // TODO WTS: Change the URL for your privacy policy in the appsettings.json file, currently set to https://YourPrivacyUrlGoesHere
    public class SettingsViewModel : ViewModelBase, INavigationAware
    {
        private readonly AppConfig _config;
        private readonly IThemeSelectorService _themeSelectorService;
        private readonly ISystemService _systemService;
        private AppTheme _theme;
        private string _versionDescription;
        private ICommand _setThemeCommand;
        private ICommand _privacyStatementCommand;

        public AppTheme Theme
        {
            get { return _theme; }
            set { Set(ref _theme, value); }
        }

        public string VersionDescription
        {
            get { return _versionDescription; }
            set { Set(ref _versionDescription, value); }
        }

        public ICommand SetThemeCommand => _setThemeCommand ?? (_setThemeCommand = new RelayCommand<string>(OnSetTheme));

        public ICommand PrivacyStatementCommand => _privacyStatementCommand ?? (_privacyStatementCommand = new RelayCommand(OnPrivacyStatement));

        public SettingsViewModel(AppConfig config, IThemeSelectorService themeSelectorService, ISystemService systemService)
        {
            _config = config;
            _themeSelectorService = themeSelectorService;
            _systemService = systemService;
        }

        public void OnNavigatedTo(object parameter)
        {
            VersionDescription = GetVersionDescription();
            Theme = _themeSelectorService.GetCurrentTheme();
        }

        public void OnNavigatedFrom()
        {
        }

        private string GetVersionDescription()
        {
            var appName = "RibbonApp";
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
    }
}
