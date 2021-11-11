using System.Threading.Tasks;
using DAL.Model;
using Microsoft.AspNetCore.Identity;

namespace DAL.UserContext
{
    public static class DataSeed
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            const string ADMIN_EMAIL = "admin@gmail.com";
            const string ADMIN_PASSWORD = "_Aa123456";
            if (await roleManager.FindByNameAsync(Roles.ADMIN) == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(Roles.ADMIN));
            }
            if (await roleManager.FindByNameAsync(Roles.USER) == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(Roles.USER));
            }

            if (await userManager.FindByNameAsync(ADMIN_EMAIL) == null)
            {
                var admin = new User
                {
                    Email = ADMIN_EMAIL, 
                    UserName = ADMIN_EMAIL,
                    EmailConfirmed = true,
                };
                var result = await userManager.CreateAsync(admin, ADMIN_PASSWORD);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.ADMIN);
                }
            }
        }
    }
}
