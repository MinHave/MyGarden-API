using Microsoft.AspNetCore.Mvc;
using MyGarden_API.Models.Entities;
using MyGarden_API.ViewModels;

namespace MyGarden_API.Services.Interfaces
{
    public interface IGardenService
    {
        Task<GardenViewModel> GetGardenById(Guid id);

        IBaseService<Garden> _baseService { get; set; }
        
        
    }
}