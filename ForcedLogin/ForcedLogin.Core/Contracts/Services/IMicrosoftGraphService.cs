using System.Threading.Tasks;

using ForcedLogin.Core.Models;

namespace ForcedLogin.Core.Contracts.Services
{
    public interface IMicrosoftGraphService
    {
        Task<User> GetUserInfoAsync(string accessToken);

        Task<string> GetUserPhoto(string accessToken);
    }
}
