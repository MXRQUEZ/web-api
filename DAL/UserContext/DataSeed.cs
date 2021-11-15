using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace DAL.UserContext
{
    public static class DataSeed
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            const string AdminEmail = "admin@gmail.com";
            const string AdminPassword = "_Aa123456";
            if (await roleManager.FindByNameAsync(Roles.ADMIN) == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(Roles.ADMIN));
            }
            if (await roleManager.FindByNameAsync(Roles.USER) == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(Roles.USER));
            }

            if (await userManager.FindByNameAsync(AdminEmail) == null)
            {
                var admin = new User
                {
                    Email = AdminEmail, 
                    UserName = AdminEmail,
                    EmailConfirmed = true,
                };
                var result = await userManager.CreateAsync(admin, AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.ADMIN);
                }
            }
        }
    }
}
