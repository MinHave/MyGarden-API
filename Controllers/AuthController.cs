﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyGarden_API.API.Auth;
using MyGarden_API.API.Helpers;
using MyGarden_API.API.Models;
using MyGarden_API.Models;
using MyGarden_API.Models.Entities;
using MyGarden_API.Services;
using MyGarden_API.Services.Interfaces;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IGardenService _gardenService;
        private readonly IUserService _userService;

        public AuthController(IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, IAuthService authService, UserManager<ApiUser> userManager, IGardenService gardenService, IUserService userService)
        {
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _authService = authService;
            _userManager = userManager;
            _gardenService = gardenService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] CredentialsViewModel credentials)
        {
            try
            {
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.AuthenticateWithCredentialsAsync(credentials.UserName, credentials.Password);

            return Ok(await GetJwtResult(response));
            }
            catch( Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("admin/login")]
        public async Task<IActionResult> adminLogin([FromBody] CredentialsViewModel credentials)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _authService.AuthenticateWithCredentialsAsync(credentials.UserName, credentials.Password);

                var roles = await _userManager.GetRolesAsync(response.User);

                if(!roles.Contains("admin")) {
                    return BadRequest();
                }

                return Ok(await GetJwtResult(response));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
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

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] string username)
        {
            bool result = await _authService.SendPasswordResetAsync(username);

            return new OkObjectResult(result);
        }

        [HttpPost("resetPasswordToken")]
        public async Task<IActionResult> ResetPasswordToken([FromBody] ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = await _authService.ResetPasswordWithTokenAsync(viewModel.Username, viewModel.Token, viewModel.Password);

            if (!result)
            {
                return BadRequest();
            }

            return new OkObjectResult(true);
        }

        [HttpPost("activateAccount")]
        public async Task<IActionResult> ActivateAccountWithToken([FromBody] ActivateAccountViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.ActivateAccount(viewModel);
            if (response)
            {
                var gardenOwner = await _userManager.FindByEmailAsync(viewModel.Email);
                if (gardenOwner == null) return BadRequest();
               
                Garden newGarden = new Garden()
                {
                    GardenOwner = gardenOwner,
                    Plants = new List<Plant>(),
                    IsDisabled = false,
                };

                var gardenCreationStatus = await _gardenService._baseService.Create(newGarden);
                if (gardenCreationStatus) return Ok(gardenCreationStatus);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.ChangePasswordAsync(viewModel.CurrentPassword, viewModel.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(ErrorHelper.AddErrorsToModelState(result, ModelState));
            }

            return Ok();
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
