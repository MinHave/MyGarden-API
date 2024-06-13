using MyGarden_API.Models.Entities;

namespace MyGarden_API.Repositories.Interfaces
{
    public interface IGardenRepository
    {
        public Task<List<Garden>> GetUserGardens(Guid userId);
    }
}
