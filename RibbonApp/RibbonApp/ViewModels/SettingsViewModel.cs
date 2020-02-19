using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using RibbonApp.Contracts.Services;
using RibbonApp.Models;

namespace RibbonApp.ViewModels
{
    // TODO WTS: Change the URL for your privacy policy in the appsettings.json file, currently set to https://YourPrivacyUrlGoesHere
    public class SettingsViewModel : BindableBase, INavigationAware
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
            set { SetProperty(ref _theme, value); }
        }

        public string VersionDescription
        {
            get { return _versionDescription; }
            set { SetProperty(ref _versionDescription, value); }
        }

        public ICommand SetThemeCommand => _setThemeCommand ?? (_setThemeCommand = new DelegateCommand<string>(OnSetTheme));

        public ICommand PrivacyStatementCommand => _privacyStatementCommand ?? (_privacyStatementCommand = new DelegateCommand(OnPrivacyStatement));

        public SettingsViewModel(AppConfig config, IThemeSelectorService themeSelectorService, ISystemService systemService)
        {
            _config = config;
            _themeSelectorService = themeSelectorService;
            _systemService = systemService;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            VersionDescription = GetVersionDescription();
            Theme = _themeSelectorService.GetCurrentTheme();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
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

        public bool IsNavigationTarget(NavigationContext navigationContext)
            => true;
    }
}
