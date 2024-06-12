using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyGarden_API.Data;
using MyGarden_API.Models.Entities;
using MyGarden_API.Services;

namespace MyGarden_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlantController : Controller
    {
        private readonly ILogger<PlantController> _logger;
        private readonly UserManager<ApiUser> _userManager;
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlants()
        {
            return await _context.Plants.ToListAsync();
        }

        // GET: Plant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(Guid id)
        {
            var plant = await _context.Plants.FindAsync(id);
            return plant == null ? NotFound() : plant;
        }

        // PUT: Garden/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlant(Guid id, Plant plant)
        {
            if (id != plant.Id) return BadRequest();

            _context.Entry(plant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        // POST: Plant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Plant>> PostPlant(Plant plant)
        {
            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPlant", new { id = plant.Id }, plant);
        }

        // DELETE: Plant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlant(Guid id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null) return NotFound();

            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool PlantExists(Guid id)
        {
            return _context.Plants.Any(e => e.Id == id);
        }
    }
}
