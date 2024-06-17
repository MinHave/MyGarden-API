using AutoMapper;
using MyGarden_API.Models.Entities;

namespace MyGarden_API.ViewModels.Mappings
{
    public class EntityToViewModelMappingProfile : Profile
    {
        public EntityToViewModelMappingProfile()
        {
            CreateMap<ApiUser, UserViewModel>()
                .ReverseMap();

            CreateMap<Plant, PlantViewModel>()
                .ReverseMap();

            CreateMap<Garden, GardenViewModel>()
                .ForMember(x => x.GardenName, z => z.MapFrom(c => c.GardenOwner.Name))
                .ReverseMap();

            CreateMap<GardenAccess, GardenAccessViewModel>()
                .ForMember(x => x.Name, z => z.MapFrom(c => c.User.Name));
        }
    }
}
