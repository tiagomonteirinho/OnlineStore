using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using OnlineStore.Data.Entities;
using OnlineStore.Models;

namespace OnlineStore.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task CheckRoleAsync(string role);

        Task AddUserToRoleAsync(User user, string role);

        Task<bool> IsUserInRoleAsync(User user, string role);

        Task<SignInResult> ValidatePasswordAsync(User user, string password); // Validate credentials without signing in.

        Task<string> GenerateEmailConfirmationTokenAsync(User user); // Generate email address confirmation token which expires after confirmation.

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<User> GetUserByIdAsync(string userId);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
    }
}
