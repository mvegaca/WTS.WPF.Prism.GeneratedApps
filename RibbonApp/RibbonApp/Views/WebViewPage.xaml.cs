using System.Windows.Controls;

using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

using RibbonApp.ViewModels;

namespace RibbonApp.Views
{
    public partial class WebViewPage : Page
    {
        private WebViewViewModel ViewModel
            => DataContext as WebViewViewModel;

        public WebViewPage()
        {
            InitializeComponent();
            ViewModel.Initialize(webView);
        }

        private void OnNavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs e)
            => ViewModel.OnNavigationCompleted(e);
    }
}
