using Microsoft.AspNetCore.Mvc;
using MyGarden_API.Models.Entities;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Services.Interfaces
{
    public interface IGardenService
    {
        public Task<GardenViewModel> GetGardenById(Guid id);

        public Task<List<Garden>> GetGardens(bool onlyActive);

        public Task<List<GardenViewModel>> GetUserGardens(Guid id);

        public Task<GardenViewModel> GetGardenFromUser(Guid id);

        public Task<List<GardenViewModel>> GetAdminGardens();

        IBaseService<Garden> _baseService { get; set; }
    }
}