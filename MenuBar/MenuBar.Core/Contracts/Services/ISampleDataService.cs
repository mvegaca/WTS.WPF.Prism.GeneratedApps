using System.Collections.Generic;
using System.Threading.Tasks;

using MenuBar.Core.Models;

namespace MenuBar.Core.Contracts.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetMasterDetailDataAsync();
    }
}
