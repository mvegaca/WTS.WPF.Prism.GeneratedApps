using System.Windows;

using Fluent;

using MahApps.Metro.Controls;

using Prism.Regions;

using RibbonApp.Constants;
using RibbonApp.Contracts.Services;

namespace RibbonApp.Views
{
    public partial class ShellWindow : MetroWindow
    {
        private RibbonTitleBar _titleBar;

        public ShellWindow(IRegionManager regionManager, IRightPaneService rightPaneService)
        {
            InitializeComponent();
            RegionManager.SetRegionName(menuContentControl, Regions.Main);
            RegionManager.SetRegionManager(menuContentControl, regionManager);
            rightPaneService.Initialize(splitView, rightPaneContentControl);
            navigationBehavior.Initialize(regionManager);
            tabsBehavior.Initialize(regionManager);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var window = sender as MetroWindow;
            _titleBar = window.FindChild<RibbonTitleBar>("RibbonTitleBar");
            _titleBar.InvalidateArrange();
            _titleBar.UpdateLayout();
        }
    }
}
