using MyGarden_API.Models.Entities;
using System.Linq.Expressions;

namespace MyGarden_API.ViewModels
{
    public class PlantViewModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string specie { get; set; }
    }
}
