using ForcedLogin.Contracts.Views;
using ForcedLogin.ViewModels;

using MahApps.Metro.Controls;

namespace ForcedLogin.Views
{
    public partial class LogInWindow : MetroWindow, ILogInWindow
    {
        public LogInWindow(LogInViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();
    }
}
