using Microsoft.AspNetCore.Identity;
using MyGarden_API.Models;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateWithCredentialsAsync(string username, string password);
        Task<AuthResponse> AuthenticateWithRefreshTokenAsync(string refreshTokenId);
        Task<IdentityResult> ChangePasswordAsync(string currentPassword, string newPassword);
        Task<bool> ResetPasswordWithTokenAsync(string username, string token, string password);
        Task<bool> SendPasswordResetAsync(string username);
        Task<IdentityResult> RegisterNewUser(UserEditViewModel viewModel);
        Task<bool> ActivateAccount(ActivateAccountViewModel viewModel);
    }
}
