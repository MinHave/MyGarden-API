using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyGarden_API.API.Auth;
using MyGarden_API.API.Models;
using MyGarden_API.Models;
using MyGarden_API.Services;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {   private readonly IAuthService _authService;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        public AuthController(IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, IAuthService authService)
        {
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.AuthenticateWithCredentialsAsync(credentials.UserName, credentials.Password);

            return Ok(await GetJwtResult(response));
        }

        [HttpPut("register")]
        public async Task<IActionResult> ItemPut([FromBody] UserEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _authService.RegisterNewUser(viewModel);

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Post([FromBody] string refreshTokenId)
        {
            var response = await _authService.AuthenticateWithRefreshTokenAsync(refreshTokenId);

            return Ok(await GetJwtResult(response));
        }


        private async Task<AuthResponseViewModel> GetJwtResult(AuthResponse response)
        {
            return new AuthResponseViewModel
            {
                Id = response.User.Id,
                Token = await _jwtFactory.GenerateEncodedToken(response.User.UserName, response.Identity),
                Expires = DateTimeOffset.Now.AddSeconds(_jwtOptions.ValidFor.TotalSeconds),
                RefreshToken = response.RefreshTokenId,
                Username = response.User.UserName,
                Name = response.User.Name,
            };
        }
    }
}
