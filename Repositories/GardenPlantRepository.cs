using Microsoft.EntityFrameworkCore;
using MyGarden_API.Data;
using MyGarden_API.Models.Entities;
using MyGarden_API.Repositories.Interfaces;

namespace MyGarden_API.Repositories
{
    public class GardenPlantRepository : IGardenPlantRepository
    {
        private readonly ApiDbContext _context;

        public GardenPlantRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Plant>> GetPlantsFromGarden(Guid? gardenId)
        {
            List<Plant> plants = await _context.Gardens
                .Where(x => x.Id == gardenId)
                .Select(x => x.Plants)
                .FirstAsync();
            return plants;
        }

        public async Task<Garden> GetGardenByPlant(Guid plantId)
        {
            Garden garden = await _context.Gardens
                .Where(x => x.Plants
                    .Any(y => y.Id == plantId))
                .FirstAsync();

            return garden;
        }
    }
}
