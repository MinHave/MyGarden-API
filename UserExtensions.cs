using MyGarden_API.API.Helpers;
using MyGarden_API.Data;
using MyGarden_API.Models.Entities;
using System.Security.Claims;

namespace MyGarden_API
{
    public static class UserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claims)
        {
            return claims.FindFirstValue(AuthConstants.JwtClaimIdentifiers.Id);
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
