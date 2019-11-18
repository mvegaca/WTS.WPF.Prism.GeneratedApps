using System.Collections.Generic;
using System.Threading.Tasks;

using Ribbon.Core.Models;

namespace Ribbon.Core.Contracts.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetMasterDetailDataAsync();
    }
}
