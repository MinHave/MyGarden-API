using MyGarden_API.Models.Entities;
using System.Security.Claims;

namespace MyGarden_API.Models
{
    public class AuthResponse
    {
        public ApiUser User { get; set; }
        public ClaimsIdentity Identity { get; set; }
        public string RefreshTokenId { get; set; }
    }
}
