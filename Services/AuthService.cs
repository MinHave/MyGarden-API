using Microsoft.AspNetCore.Identity;
using MyGarden_API.API.Auth;
using MyGarden_API.Models.Entities;
using System.Security.Claims;

namespace MyGarden_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly ApiDbContext _context;
        private readonly ILogger<AuthModel> _logger;
        private readonly ClaimsPrincipal _user;
        private readonly IMailService _mailService;
    }
}
