using MyGarden_API.Models.Entities.Enums;

namespace MyGarden_API.Models.Entities
{
    public class GardenAccess
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public Access Access { get; set; }

        public ApiUser User { get; set; }

        public Garden Garden { get; set; }
    }
}
