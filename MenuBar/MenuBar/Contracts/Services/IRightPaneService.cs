using System;
using System.Windows.Controls;

using MahApps.Metro.Controls;

using Prism.Regions;

namespace MenuBar.Contracts.Services
{
    public interface IRightPaneService
    {
        event EventHandler PaneOpened;

        event EventHandler PaneClosed;

        void OpenInRightPane(string pageKey, NavigationParameters navigationParameters = null);

        void Initialize(SplitView splitView, ContentControl rightPaneContentControl);

        void CleanUp();
    }
}
