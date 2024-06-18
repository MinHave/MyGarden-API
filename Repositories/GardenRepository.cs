using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyGarden_API.Data;
using MyGarden_API.Models.Entities;
using MyGarden_API.Models.Entities.Enums;
using MyGarden_API.Repositories.Interfaces;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Repositories
{
    public class GardenRepository : IGardenRepository
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;

        public GardenRepository(ApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GardenViewModel> GetGardenById(Guid gardenId)
        {
            var garden = await _context.Gardens
                .Where(x => x.Id == gardenId)
                .Include(x => x.GardenOwner)
                .Include(x => x.Plants)
                .FirstOrDefaultAsync();

            return _mapper.Map<GardenViewModel>(garden);
        }

        public async Task<List<GardenViewModel>> GetUserGardens(Guid userId)
        {
            var gardenList = await _context.Gardens
                .WhereAllowedByUser(userId.ToString(), _context, Access.GetGarden)
                .Include(x => x.GardenOwner)
                .Include(x => x.Plants)
                .ToListAsync();

            List<GardenViewModel> gardenAccessList = [];

            foreach (Garden item in gardenList)
            {
                gardenAccessList.Add(_mapper.Map<GardenViewModel>(item));
            }

            return gardenAccessList;
        }

        public async Task<List<GardenViewModel>> GetAll()
        {
            var gardenList = await _context.Gardens
                .Include(x => x.GardenOwner)
                .Include(x => x.Plants)
                .ToListAsync();

            List<GardenViewModel> gardenAccessList = [];

            foreach (Garden item in gardenList)
            {
                gardenAccessList.Add(_mapper.Map<GardenViewModel>(item));
            }

            return gardenAccessList;
        }
    }
}
