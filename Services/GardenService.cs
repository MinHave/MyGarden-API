using AutoMapper;
using MyGarden_API.Models.Entities;
using MyGarden_API.Repositories;
using MyGarden_API.Repositories.Interfaces;
using MyGarden_API.Services.Interfaces;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Services
{
    public class GardenService : IGardenService
    {
        private readonly IRepositoryDesignPattern<Garden> _designPattern;
        private readonly IGardenPlantRepository _gardenPlantRepository;
        private readonly IMapper _mapper;

        public IBaseService<Garden> _baseService { get; set; }

        public GardenService(IRepositoryDesignPattern<Garden> designPattern, IBaseService<Garden> baseService, IGardenPlantRepository gardenPlantRepository, IMapper mapper)
        {
            _designPattern = designPattern;
            _baseService = baseService;
            _gardenPlantRepository = gardenPlantRepository;
            _mapper = mapper;
        }

        public async Task<List<Garden>> GetGardens(bool onlyActive)
        {
            var result = await _designPattern.GetAll<Garden>(
                disabledCondition => disabledCondition.IsDisabled == onlyActive,
                onlyActive
                );
            return result.ToList();
        }

        public async Task<GardenViewModel> GetGardenById(Guid id)
        {
            Garden garden = await _designPattern.GetByCondition<Garden>(
                 condition => condition.Id == id,
                 disabledCondition => true,
                 true
             );

            return _mapper.Map<GardenViewModel>(garden);
        }

        public async Task<GardenViewModel> GetGardenFromUser(Guid id)
        {
            Garden garden = await _designPattern.GetByCondition<Garden>(
                 condition => id.Equals(condition.GardenOwner.Id),
                 disabledCondition => true,
                 true
             );

            return _mapper.Map<GardenViewModel>(garden);
        }
    }
}
