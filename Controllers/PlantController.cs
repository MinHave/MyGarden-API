using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGarden_API.Data;
using MyGarden_API.Models.Entities;
using MyGarden_API.Repositories;
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

        //private readonly ILogger<PlantController> _logger;
        private readonly IPlantService _plantService;


        public PlantController(IPlantService plantService)
        {
            _plantService = plantService;
            //_logger = logger;
        }

        [HttpGet("gardenPlants{gardenId}")]
        public async Task<ActionResult<List<PlantViewModel>>> GetPlants(Guid gardenId)
        {
            return await _plantService.GetPlantsFromGarden(gardenId);
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
        public async Task<ActionResult<Plant>> CreatePlant(Plant plant)
        {
            await _plantService._baseService.Create(plant);
            //_context.Plants.Add(plant);
            return CreatedAtAction("GetPlant", new { id = plant.Id }, plant);
        }

        // DELETE: Plant
        [HttpDelete]
        public async Task<IActionResult> DeletePlant(Plant plant)
        {
            if (plant == null) return NotFound();
            var result = await _designPattern.Delete(plant);
            return result ? Ok() : BadRequest();
        }
    }
}
