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
        }
    }
}
