using MyGarden_API.Data;
using MyGarden_API.Models.Entities;
using MyGarden_API.Models.Entities.Enums;
using System.Security.Claims;

namespace MyGarden_API
{
    public static class DataExtensions
    {
        public static IQueryable<Garden> WhereAllowedByUser(this IQueryable<Garden> query, string userId, ApiDbContext context, Access modifier)
        {
            var gardenAccess = context.GardenAccess
                .Where(x => x.UserId == userId && x.Access.HasFlag(modifier))
                .Select(x => x.Garden.Id)
                .ToList();

            return query.Where(x => gardenAccess.Contains(x.Id));
        }
    }
}
