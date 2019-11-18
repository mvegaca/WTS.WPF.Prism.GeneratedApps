using System.Windows.Controls;

using Blank.ViewModels;

using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Blank.Views
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
