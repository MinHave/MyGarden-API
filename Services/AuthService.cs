using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyGarden_API.API.Auth;
using MyGarden_API.Data;
using MyGarden_API.Models;
using MyGarden_API.Models.Entities;
using MyGarden_API.ViewModels;
using System.Security.Claims;

namespace MyGarden_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly ApiDbContext _context;
        private readonly ILogger<AuthService> _logger;
        private readonly ClaimsPrincipal _user;
        private readonly IMailService _mailService;

        public AuthService(UserManager<ApiUser> userManager, IJwtFactory jwtFactory, ApiDbContext context, ILogger<AuthService> logger, ClaimsPrincipal user, IMailService mailService)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _context = context;
            _logger = logger;
            _user = user;
            _mailService = mailService;
        }


        public async Task<AuthResponse> AuthenticateWithCredentialsAsync(string username, string password)
        {
            ClaimsIdentity identity = null;
            IList<string> roles = null;

            var user = await _userManager.Users
                .Where(x => x.NormalizedUserName == username).FirstOrDefaultAsync();

            if (user != null)
            {
                if (user.IsDisabled)
                {
                    _logger.LogWarning("User {username} {userId} is disabled", user.UserName, user.Id);
                    throw new InvalidOperationException("User is disabled");
                }

                // check the credentials
                if (await _userManager.CheckPasswordAsync(user, password))
                {
                    _logger.LogInformation("Valid login for user {username} {userId}", user.UserName, user.Id);

                    roles = await _userManager.GetRolesAsync(user);

                    identity = _jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id, roles);
                }
                else
                {
                    _logger.LogInformation("Wrong password for {username} {userId}", user.UserName, user.Id);
                }
            }
            else
            {
                _logger.LogInformation("Unknown user login attempt {username}", username);
            }

            if (identity == null)
            {
                throw new InvalidOperationException("Invalid username or password");
            }

            var refreshToken = _context.AuthRefreshTokens.Where(x => x.User == user && x.Enabled).FirstOrDefault();

            if (refreshToken == null)
            {
                refreshToken = new AuthRefreshToken()
                {
                    User = user,
                    Enabled = true
                };

                _context.AuthRefreshTokens.Add(refreshToken);

                _context.SaveChanges();
            }

            return new AuthResponse()
            {
                User = user,
                Identity = identity,
                RefreshTokenId = Convert.ToBase64String(refreshToken.Id.ToByteArray()),
            };
        }

        public async Task<bool> ActivateAccount(ActivateAccountViewModel viewModel)
        {
            ApiUser user = _context.Users.Where(x => x.UserName == viewModel.Email).FirstOrDefault();

            if (user == null)
            {
                //throw new Exception("No user with email found");
                throw new ModelResponse<bool>()
                {
                    HTTP = StatusCodes.Status400BadRequest,
                    Title = "No user with email found",
                    CustomMessage = "A user with provided email could not be found",
                    Value = false,
                };
            }

            var result = await _userManager.ChangePasswordAsync(user, viewModel.ActivationToken, viewModel.Password);

            if (!result.Succeeded)
            {
                throw new ModelResponse<bool>()
                {
                    HTTP = StatusCodes.Status400BadRequest,
                    Title = "Account activation failed",
                    CustomMessage = result.Errors.ToString(),
                    Value = false,
                };
            }

            user.EmailConfirmed = true;

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<AuthResponse> AuthenticateWithRefreshTokenAsync(string refreshTokenId)
        {
            Guid targetGuid = new(Convert.FromBase64String(refreshTokenId));

            var token = await _context.AuthRefreshTokens
                .Include(x => x.User)
                .Where(x => x.Id == targetGuid)
                .FirstOrDefaultAsync();

            if (token == null)
            {
                _logger.LogWarning("Unknown refresh token {tokenId}", refreshTokenId);

                throw new NotAuthorizedException();
            }

            if (!token.Enabled)
            {
                _logger.LogWarning("Attempt to use refresh token {tokenId} for user {username} that is not enabled", refreshTokenId, token.User.UserName);

                throw new NotAuthorizedException();
            }

            _logger.LogDebug("Generating token for user {username} {userId} from valid refresh token {tokenId}", token.User.UserName, token.User.Id, token.Id);

            IList<string> roles = await _userManager.GetRolesAsync(token.User);

            return new AuthResponse()
            {
                User = token.User,
                Identity = _jwtFactory.GenerateClaimsIdentity(token.User.UserName, token.User.Id, roles),
                RefreshTokenId = Convert.ToBase64String(token.Id.ToByteArray()),
            };
        }

        public async Task<IdentityResult> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            var user = _user.GetUser(_context);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            _logger.LogInformation("User {username} password change. Succeeded: {success}", user.UserName, result.Succeeded);

            return result;
        }

        public async Task<bool> SendPasswordResetAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                _logger.LogWarning("Cannot send reset password to user with username {username} that could not be found", user);

                return false;
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _mailService.SendPasswordResetEmailAsync(user.Email, user.Name ?? user.Email, token);

            _logger.LogInformation("Sent password reset token for {userId} {email}", user.Id, user.Email);

            return true;
        }

        public async Task<bool> ResetPasswordWithTokenAsync(string username, string token, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                _logger.LogWarning("Cannot reset password with token for username {username} that could not be found", username);
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, password);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Bad request for password reset with token for {userId} {email}: {error}", user.Id, user.Email, string.Join(", ", result.Errors.Select(x => x.Description)));
                return false;
            }

            _logger.LogInformation("Password reset for {userId} {email} with token", user.Id, user.Email);

            return true;
        }

        public async Task<IdentityResult> RegisterNewUser(UserEditViewModel viewModel)
        {
            ApiUser user = new ApiUser()
            {
                Email = viewModel.Email,
                Name = viewModel.Name,
                PhoneNumber = viewModel.PhoneNumber,
                UserName = viewModel.Email,
            };

            string userPassword = GeneratePassword();

            IdentityResult result = await _userManager.CreateAsync(user, userPassword);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Unable to create user");
            }

            await _mailService.SendNewUserEmailAsync(user.Email, user.Name ?? user.Email, userPassword);

            _logger.LogInformation("New user was added with email {email}", user.Name);

            await _context.SaveChangesAsync();


            return result;
        }

        private static readonly Random Random = new();

        private static string GeneratePassword()
        {
            byte[] buffer = new byte[48];
            Random.NextBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

    }
}
