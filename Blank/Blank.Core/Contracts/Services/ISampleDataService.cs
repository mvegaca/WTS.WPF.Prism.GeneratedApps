using System.Collections.Generic;
using System.Threading.Tasks;

using Blank.Core.Models;

namespace Blank.Core.Contracts.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetMasterDetailDataAsync();
    }
}
