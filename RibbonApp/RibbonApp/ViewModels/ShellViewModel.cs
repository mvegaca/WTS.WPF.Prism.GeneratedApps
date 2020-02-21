using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using RibbonApp.Constants;
using RibbonApp.Contracts.Services;

namespace RibbonApp.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IRightPaneService _rightPaneService;
        private IRegionNavigationService _navigationService;
        private ICommand _loadedCommand;
        private ICommand _unloadedCommand;

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new DelegateCommand(OnLoaded));

        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new DelegateCommand(OnUnloaded));

        public ShellViewModel(IRegionManager regionManager, IRightPaneService rightPaneService)
        {
            _regionManager = regionManager;
            _rightPaneService = rightPaneService;
        }

        private void OnLoaded()
        {
            _navigationService = _regionManager.Regions[Regions.Main].NavigationService;
            _navigationService.RequestNavigate(PageKeys.Main);
        }

        private void OnUnloaded()
        {
            _regionManager.Regions.Remove(Regions.Main);
            _rightPaneService.CleanUp();
        }
    }
}
