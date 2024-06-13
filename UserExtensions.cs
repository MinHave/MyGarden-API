using MyGarden_API.API.Helpers;
using MyGarden_API.Data;
using MyGarden_API.Models.Entities;
using MyGarden_API.Models.Entities.Enums;
using System.Security.Claims;

namespace MyGarden_API
{
    public static class UserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claims)
        {
            return claims.FindFirstValue(AuthConstants.JwtClaimIdentifiers.Id);
        }
        public static bool HasPermission(this ClaimsPrincipal claims, ApiDbContext context, Guid gardenId, Access gardenAccess)
        {
            return context.GardenAccess.Where(x => x.UserId == claims.GetUserId() && x.Garden.Id == gardenId && x.Access.HasFlag(gardenAccess)).Any();
        }

        public static ApiUser GetUser(this ClaimsPrincipal claims, ApiDbContext context)
        {
            var user = context.Users.Find(claims.GetUserId());

            if (user == null)
            {
                throw new InvalidOperationException($"User {claims.GetUserId()} not found");
            }

            return user;
        }
    }
}
