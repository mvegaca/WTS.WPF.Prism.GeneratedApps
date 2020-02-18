using System.Threading.Tasks;

using NavigationPane.Core.Models;

namespace NavigationPane.Core.Contracts.Services
{
    public interface IMicrosoftGraphService
    {
        Task<User> GetUserInfoAsync(string accessToken);

        Task<string> GetUserPhoto(string accessToken);
    }
}
