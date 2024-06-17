using MyGarden_API.Models.Entities;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Repositories.Interfaces
{
    public interface IGardenRepository
    {
        public Task<List<GardenViewModel>> GetUserGardens(Guid userId);
    }
}
