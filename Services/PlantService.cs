using AutoMapper;
using MyGarden_API.Models.Entities;
using MyGarden_API.Repositories;
using MyGarden_API.Repositories.Interfaces;
using MyGarden_API.Services.Interfaces;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Services
{
    public class PlantService : IPlantService
    {
        private readonly IRepositoryDesignPattern<Plant> _designPattern;
        private readonly IGardenPlantRepository _gardenPlantRepository;
        private readonly IMapper _mapper;

        public IBaseService<Plant> _baseService { get; set; }

        public PlantService(IRepositoryDesignPattern<Plant> designPattern, IBaseService<Plant> baseService, IGardenPlantRepository gardenPlantRepository, IMapper mapper)
        {
            _designPattern = designPattern;
            _baseService = baseService;
            _gardenPlantRepository = gardenPlantRepository;
            _mapper = mapper;
        }

        public async Task<List<PlantViewModel>> GetPlantsFromGarden(Guid gardenId)
        {
            List<Plant> gardenPlants = await _gardenPlantRepository.GetPlantsFromGarden(gardenId);
            return _mapper.Map<List<PlantViewModel>>(gardenPlants);
        }

        public async Task<PlantViewModel> GetPlantById(Guid plantId)
        {
            Plant plant = await _designPattern.GetByCondition<Plant>(
                condition => condition.Id == plantId,
                disabledCondition => true,
                true
            );

            return _mapper.Map<PlantViewModel>(plant);
        }
    }
}
