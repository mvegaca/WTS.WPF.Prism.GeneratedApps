using System.Threading.Tasks;

using RibbonApp.Core.Models;

namespace RibbonApp.Core.Contracts.Services
{
    public interface IMicrosoftGraphService
    {
        Task<User> GetUserInfoAsync(string accessToken);

        Task<string> GetUserPhoto(string accessToken);
    }
}
