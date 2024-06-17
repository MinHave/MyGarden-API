using MyGarden_API.Models.Entities;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Services.Interfaces
{
    public interface IPlantService
    {
        IBaseService<Plant> _baseService { get; set; }
        public Task<List<PlantViewModel>> GetPlantsFromGarden(Guid? gardenId);
        public Task<PlantViewModel> GetPlantById(Guid plantId);
    }
}
