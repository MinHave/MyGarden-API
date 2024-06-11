using MyGarden_API.Models.Entities;

namespace MyGarden_API.ViewModels
{
    public class GardenAccessViewModel
    {
        public string Name { get; set; }

        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public Guid GardenId { get; set; }

        public GardenAccess GardenAccess { get; set; }
    }
}
