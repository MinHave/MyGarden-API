using Microsoft.EntityFrameworkCore;
using MyGarden_API.Data;
using MyGarden_API.Models.Entities;
using MyGarden_API.Models.Entities.Enums;
using MyGarden_API.Repositories.Interfaces;

namespace MyGarden_API.Repositories
{
    public class GardenRepository : IGardenRepository
    {
        private readonly ApiDbContext _context;

        public GardenRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Garden>> GetUserGardens(Guid userId)
        {
            List<Garden> gardenList = await _context.Gardens
                .WhereAllowedByUser(userId.ToString(), _context, Access.GetGarden)
                .ToListAsync();
            return gardenList;
        }
    }
}
