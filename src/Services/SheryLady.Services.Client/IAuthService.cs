﻿namespace SheryLady.Services.Client
{
    using System.Threading.Tasks;

    using Web.Shared.Identity;

    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginRequestModel model);

        Task LogoutAsync();
    }
}
