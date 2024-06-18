using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    //[Authorize(Policy = AuthPolicies.RequireAdmin)]
    public class UserController : Controller
    {
        private readonly IBaseService<ApiUser> _baseService;
        private readonly IUserService _userService;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(IBaseService<ApiUser> baseService, IUserService userService, UserManager<ApiUser> userManager, IMapper mapper)
        {
            _baseService = baseService;
            _userService = userService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserViewModel>>> GetUserList()
        {
            var userId = User.GetUserId();
            var apiUser = await _userService.GetUserById(userId);
            if (apiUser == null) return BadRequest();

            var roles = await _userManager.GetRolesAsync(_mapper.Map<ApiUser>(apiUser));

            if (!roles.Contains("admin"))
            {
                return BadRequest();
            }

            List<UserViewModel> users = await _userService.GetUsers();
                return users;
 
        }

        // GET: user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> GetUser(string id)
        {
            var userId = User.GetUserId();
            var apiUser = await _userService.GetUserById(userId);
            if (apiUser == null) return BadRequest();

            var roles = await _userManager.GetRolesAsync(_mapper.Map<ApiUser>(apiUser));

            if (!roles.Contains("admin"))
            {
                return BadRequest();
            }
            UserViewModel user = await _userService.GetUserById(id);

            return user;
        }


        // DELETE: Garden
        [HttpPost]
        public async Task<IActionResult> ToggleUser(string id)
        {
            var userId = User.GetUserId();
            var apiUser = await _userService.GetUserById(userId);
            if (apiUser == null) return BadRequest();

            var roles = await _userManager.GetRolesAsync(_mapper.Map<ApiUser>(apiUser));

            if (!roles.Contains("admin"))
            {
                return BadRequest();
            }
            var success = await _userService.ToggleUserDisabled(id);

            return success ? Ok(success) : BadRequest();
            
        }
    }
}
