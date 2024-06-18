using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyGarden_API.API.Helpers;
using MyGarden_API.Models.Entities;
using MyGarden_API.Services;
using MyGarden_API.Services.Interfaces;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = AuthPolicies.RequireAdmin)]
    public class UserController : Controller
    {
        private readonly IBaseService<ApiUser> _baseService;
        private readonly IUserService _userService;
        public UserController(IBaseService<ApiUser> baseService, IUserService userService)
        {
            _baseService = baseService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserViewModel>>> GetUserList()
        {
            List<UserViewModel> users = await _userService.GetUsers();
            return users;
            
        }

        // GET: user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> GetUser(string id)
        {
            UserViewModel user = await _userService.GetUserById(id);

            return user;
        }


        // DELETE: Garden
        [HttpPost]
        public async Task<IActionResult> ToggleUser(string id)
        {
            var success = await _userService.ToggleUserDisabled(id);

            return success ? Ok(success) : BadRequest();
            
        }
    }
}
