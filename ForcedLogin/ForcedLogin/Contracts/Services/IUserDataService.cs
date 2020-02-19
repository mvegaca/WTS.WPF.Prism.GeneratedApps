using System;

using ForcedLogin.ViewModels;

namespace ForcedLogin.Contracts.Services
{
    public interface IUserDataService
    {
        event EventHandler<UserViewModel> UserDataUpdated;

        void Initialize();

        UserViewModel GetUser();
    }
}
