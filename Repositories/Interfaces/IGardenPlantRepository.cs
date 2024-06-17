using MyGarden_API.Models.Entities;

namespace MyGarden_API.Repositories.Interfaces
{
    public interface IGardenPlantRepository
    {
        public Task<List<Plant>> GetPlantsFromGarden(Guid? gardenId);
        public Task<Garden> GetGardenByPlant(Guid plantId);
    }
}
