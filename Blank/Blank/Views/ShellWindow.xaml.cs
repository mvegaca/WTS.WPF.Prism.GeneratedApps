using Blank.Constants;

using MahApps.Metro.Controls;

using Prism.Regions;

namespace Blank.Views
{
    public partial class ShellWindow : MetroWindow
    {
        public ShellWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionName(shellContentControl, Regions.Main);
            RegionManager.SetRegionManager(shellContentControl, regionManager);
        }
    }
}
