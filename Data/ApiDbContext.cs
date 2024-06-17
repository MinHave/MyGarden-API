using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyGarden_API.Models;
using MyGarden_API.Models.Entities;

namespace MyGarden_API.Data
{
    public class ApiDbContext : IdentityDbContext<ApiUser>
    {
        public ApiDbContext(DbContextOptions options) : base(options)
        {
            SavingChanges += ApiDbContext_SavingChanges;
        }

        public DbSet<AuthRefreshToken> AuthRefreshTokens { get; set; }

        public DbSet<Garden> Gardens { get; set; }

        public DbSet<GardenAccess> GardenAccess { get; set; }

        public DbSet<ApiUser> ApiUsers { get; set; }

        public DbSet<Plant> Plants { get; set; }

        public DbSet<Image> Images { get; set; }

        private void ApiDbContext_SavingChanges(object sender, SavingChangesEventArgs e)
        {
            UpdateCreatedChanged();
        }

        private void UpdateCreatedChanged()
        {
            var now = DateTimeOffset.Now;
            foreach (var entry in ChangeTracker.Entries<IDatedEntity>())
            {
                var entity = entry.Entity;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.Created = now;
                        entity.Updated = now;
                        break;

                    case EntityState.Modified:
                        entity.Updated = now;
                        break;
                }
            }

            ChangeTracker.DetectChanges();
        }
    }
}
