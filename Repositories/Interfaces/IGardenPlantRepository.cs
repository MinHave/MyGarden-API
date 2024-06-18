using MyGarden_API.Models.Entities;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Repositories.Interfaces
{
    public interface IGardenPlantRepository
    {
        public Task<List<Plant>> GetPlantsFromGarden(Guid? gardenId);
        public Task<Garden> GetGardenByPlant(Guid plantId);
        public Task<PlantViewModel> AddPlantToGarden(PlantViewModel newPlant, Guid gardenId);
    }
}
