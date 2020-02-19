using System.Threading.Tasks;

namespace RibbonApp.Contracts.Services
{
    public interface IApplicationHostService
    {
        Task StartAsync();

        Task StopAsync();
    }
}
