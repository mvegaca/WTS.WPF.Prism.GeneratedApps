using System.Collections.Generic;
using System.Threading.Tasks;

using NavigationPane.Core.Models;

namespace NavigationPane.Core.Contracts.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetMasterDetailDataAsync();
    }
}
