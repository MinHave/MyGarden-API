using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGarden_API.Data;
using MyGarden_API.Models.Entities;
using MyGarden_API.Services;
using MyGarden_API.Services.Interfaces;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class GardenController : Controller
    {
        private readonly IGardenService _gardenService;
        private readonly IUserService _userService;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IMapper _mapper;

        public GardenController(IGardenService gardenService, IUserService userService, UserManager<ApiUser> userManager, IMapper mapper)
        {
            _gardenService = gardenService;
            _userService = userService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GardenViewModel>>> GetGardenList()
        {
            var userId = User.GetUserId();
            var apiUser = await _userService.GetUserById(userId);
            if (apiUser == null) return BadRequest();

            var roles = await _userManager.GetRolesAsync(_mapper.Map<ApiUser>(apiUser));

            if (roles.Contains("admin"))
            {
                return await _gardenService.GetAdminGardens();
            }
            else
            {
                return await _gardenService.GetUserGardens(Guid.Parse(userId));
            }
        }
   
        // GET: garden/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GardenViewModel>> GetGarden(Guid id)
        {

            var garden = await _gardenService.GetGardenById(id);
            return garden == null ? NotFound() : garden;
        }

        // PUT: Garden
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutGarden(Garden garden)
        {
            var result = await _gardenService._baseService.Update(garden);
            return result ? Ok() : BadRequest();
        }

        // POST: Garden
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Garden>> CreateGarden(Garden garden)
        {
            await _gardenService._baseService.Create(garden);
            //_context.Gardens.Add(garden);
            return CreatedAtAction("GetGarden", new { id = garden.Id }, garden);
        }

        // DELETE: Garden
        [HttpDelete]
        public async Task<IActionResult> DeleteGarden(Garden garden)
        {
            if (garden == null) return NotFound();
            var result = await _gardenService._baseService.Delete(garden);
            return result ? Ok() : BadRequest();
        }

        // Toggle: garden
        [HttpPost("toggle/{gardenId}")]
        public async Task<IActionResult> ToggleGarden(Guid gardenId)
        {
            var success = await _gardenService.ToggleGardenDisabled(gardenId);

            return success ? Ok(success) : BadRequest();

        }
    }
}
