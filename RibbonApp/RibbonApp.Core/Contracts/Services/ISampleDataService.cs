using System.Collections.Generic;
using System.Threading.Tasks;

using RibbonApp.Core.Models;

namespace RibbonApp.Core.Contracts.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetMasterDetailDataAsync();
    }
}
