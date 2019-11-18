using System.Windows.Controls;

using MenuBar.ViewModels;

using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace MenuBar.Views
{
    public partial class WebViewPage : UserControl
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
