using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGarden_API.Data;
using MyGarden_API.Models.Entities;
using MyGarden_API.Repositories;
using MyGarden_API.Repositories.Interfaces;
using MyGarden_API.Services;
using MyGarden_API.Services.Interfaces;
using MyGarden_API.ViewModels;
using System.Net.Sockets;

namespace MyGarden_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlantController : Controller
    {
        private readonly IRepositoryDesignPattern<Plant> _designPattern;

        private readonly IPlantService _plantService;
        private readonly IGardenPlantRepository _gardenPlantRepository;


        public PlantController(IPlantService plantService, IGardenPlantRepository gardenPlantRepository)
        {
            _plantService = plantService;
            _gardenPlantRepository = gardenPlantRepository;
        }

        [HttpGet("gardenPlants/{gardenId}")]
        public async Task<ActionResult<List<PlantViewModel>>> GetPlants(Guid? gardenId)
        {
            List<PlantViewModel> plants = await _plantService.GetPlantsFromGarden(gardenId);

            return plants;
        }

        // GET: Plant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlantViewModel>> GetPlant(Guid id)
        {
            //var plant = await _context.Plants.FindAsync(id);
            var plant = await _plantService.GetPlantById(id);
            return plant == null ? NotFound() : plant;
        }

        // PUT: Plant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutPlant(Plant plant)
        {
            var result = await _plantService._baseService.Update(plant);
            return result ? Ok() : BadRequest();
        }

        // POST: Plant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Plant>> CreatePlant(PlantViewModel plant)
        {
            //var newPlant = _mapper.Map<Plant>(plant);
            //await _plantService._baseService.Create(newPlant);
            await _gardenPlantRepository.AddPlantToGarden(plant, Guid.Parse(plant.gardenId));
            return CreatedAtAction("GetPlant", new { id = plant.id}, plant);
        }

        // DELETE: Plant
        [HttpDelete]
        public async Task<IActionResult> DeletePlant([FromBody] Plant plant)
        {
            if (plant == null) return NotFound();
            var result = await _plantService._baseService.Delete(plant);
            return result ? Ok() : BadRequest();
        }
    }
}
