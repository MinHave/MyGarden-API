using Microsoft.AspNetCore.Identity;

namespace MyGarden_API.Models.Entities
{
    public class ApiUser : IdentityUser, IDatedEntity, IDisabledEntity
    {
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Updated { get; set; }

        public bool IsDisabled { get; set; }
    
        public string Name { get; set; }
    }
}
