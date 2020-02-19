using System;
using System.Windows.Controls;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using Microsoft.Extensions.Configuration;

using RibbonApp.Contracts.Services;
using RibbonApp.Contracts.Views;
using RibbonApp.Core.Contracts.Services;
using RibbonApp.Core.Services;
using RibbonApp.Models;
using RibbonApp.Services;
using RibbonApp.Views;

namespace RibbonApp.ViewModels
{
    public class ViewModelLocator
    {
        private IPageService PageService
            => SimpleIoc.Default.GetInstance<IPageService>();

        public ShellViewModel ShellViewModel
            => SimpleIoc.Default.GetInstance<ShellViewModel>();

        public MainViewModel MainViewModel
            => SimpleIoc.Default.GetInstance<MainViewModel>();

        public MasterDetailViewModel MasterDetailViewModel
            => SimpleIoc.Default.GetInstance<MasterDetailViewModel>();

        public WebViewViewModel WebViewViewModel
            => SimpleIoc.Default.GetInstance<WebViewViewModel>();

        public SettingsViewModel SettingsViewModel
            => SimpleIoc.Default.GetInstance<SettingsViewModel>();

        public ViewModelLocator()
        {
            // App Host
            SimpleIoc.Default.Register<IApplicationHostService, ApplicationHostService>();

            // Core Services
            SimpleIoc.Default.Register<ISystemService, SystemService>();
            SimpleIoc.Default.Register<ISampleDataService, SampleDataService>();
            SimpleIoc.Default.Register<IFileService, FileService>();

            // Services
            SimpleIoc.Default.Register<IWindowManagerService, WindowManagerService>();
            SimpleIoc.Default.Register<IPersistAndRestoreService, PersistAndRestoreService>();
            SimpleIoc.Default.Register<IThemeSelectorService, ThemeSelectorService>();
            SimpleIoc.Default.Register<IRightPaneService, RightPaneService>();
            SimpleIoc.Default.Register<IPageService, PageService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();

            // Window
            SimpleIoc.Default.Register<IShellWindow, ShellWindow>();
            SimpleIoc.Default.Register<ShellViewModel>();

            // Pages
            Register<MainViewModel, MainPage>();
            Register<MasterDetailViewModel, MasterDetailPage>();
            Register<WebViewViewModel, WebViewPage>();
            Register<SettingsViewModel, SettingsPage>();
        }

        private void Register<VM, V>()
            where VM : ViewModelBase
            where V : Page
        {
            SimpleIoc.Default.Register<VM>();
            SimpleIoc.Default.Register<V>();
            PageService.Configure<VM, V>();
        }

        public void AddConfiguration(IConfiguration configuration)
        {
            var appConfig = configuration
                .GetSection(nameof(AppConfig))
                .Get<AppConfig>();

            // Register configurations to IoC
            SimpleIoc.Default.Register(() => configuration);
            SimpleIoc.Default.Register(() => appConfig);
        }
    }
}
