using Microsoft.AspNetCore.Identity;
using MyGarden_API.API.Helpers;
using MyGarden_API.Models.Entities;

namespace MyGarden_API.Data
{
    public class DefaultData
    {
        private static ApiUser GenerateUserObject(string username, string name)
        {
            return new ApiUser()
            {
                UserName = username,
                Email = username,
                Name = name
            };
        }
        private static async Task AddUser(UserManager<ApiUser> userMgr, string username, string name)
        {
            var user = GenerateUserObject(username, name);
            await userMgr.CreateAsync(user, "P@ssw0rd");
        }

        private static async Task AddUserWithAllPerms(UserManager<ApiUser> userMgr, string username, string name)
        {
            var user = GenerateUserObject(username, name);
            await userMgr.CreateAsync(user, "P@ssw0rd");
            await userMgr.AddToRoleAsync(user, AuthRoles.Admin);
        }

        public static async Task PopulateAccounts(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApiDbContext>();
            var userMgr = serviceProvider.GetRequiredService<UserManager<ApiUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            if (!await roleManager.RoleExistsAsync(AuthRoles.Admin)) await roleManager.CreateAsync(new IdentityRole() { Name = AuthRoles.Admin });
            if (!await roleManager.RoleExistsAsync(AuthRoles.Manager)) await roleManager.CreateAsync(new IdentityRole() { Name = AuthRoles.Manager });

            if (!context.Users.Any())
            {
                await AddUserWithAllPerms(userMgr, "mgc@tavsogmatias.com", "Matias Grimm");
                await AddUser(userMgr, "tma@tavsogmatias.com", "Tavs Malling");
            }
        }
    }
}
