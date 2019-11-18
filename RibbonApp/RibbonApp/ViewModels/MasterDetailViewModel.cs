using System;
using System.Collections.ObjectModel;
using System.Linq;

using Prism.Mvvm;
using Prism.Regions;

using RibbonApp.Core.Contracts.Services;
using RibbonApp.Core.Models;

namespace RibbonApp.ViewModels
{
    public class MasterDetailViewModel : BindableBase, INavigationAware
    {
        private readonly ISampleDataService _sampleDataService;
        private SampleOrder _selected;

        public SampleOrder Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

        public MasterDetailViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            SampleItems.Clear();

            var data = await _sampleDataService.GetMasterDetailDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }

            Selected = SampleItems.First();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
            => true;
    }
}
