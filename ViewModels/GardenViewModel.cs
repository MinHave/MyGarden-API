using MyGarden_API.Models.Entities;

namespace MyGarden_API.ViewModels
{
    public class GardenViewModel
    {
        public Guid Id { get; set; }

        public List<PlantViewModel> Plants { get; set; }

        //public UserViewModel GardenOwner { get; set; }

        public string GardenName { get; set; }

        public bool IsDisabled { get; set; }
    }
}
