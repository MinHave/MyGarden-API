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
            return query.Where(x => x.GardenOwner.Id == userId || context.GardenAccess
                .Where(ga => ga.UserId == userId && (ga.Access & modifier) == modifier)
                .Select(ga => ga.Garden.Id)
                .Contains(x.Id));
        }
    }
}
