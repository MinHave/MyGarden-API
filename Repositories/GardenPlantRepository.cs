using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyGarden_API.Data;
using MyGarden_API.Models.Entities;
using MyGarden_API.Repositories.Interfaces;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Repositories
{
    public class GardenPlantRepository : IGardenPlantRepository
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public GardenPlantRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Plant>> GetPlantsFromGarden(Guid? gardenId)
        {
            List<Plant> plants = await _context.Gardens
                .Where(x => x.Id == gardenId)
                .Select(x => x.Plants)
                .FirstAsync();
            return plants;
        }

        public async Task<PlantViewModel> AddPlantToGarden(PlantViewModel newPlant, Guid gardenId)
        {
            Garden? garden = await _context.Gardens
                .Where(x => x.Id == gardenId)
                .Include(x => x.Plants)
                .FirstOrDefaultAsync();

            //var a = _context.Plants.Add(_mapper.Map<Plant>(newPlant));

            if (garden != null) 
            {
                garden.Plants.Add(_mapper.Map<Plant>(newPlant));
                await _context.SaveChangesAsync();
                return newPlant;
            }
            return null;
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
