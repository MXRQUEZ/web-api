using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace DAL.UserContext
{
    public static class UsersDataSeed
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            const string AdminEmail = "admin@gmail.com";
            const string UserEmail = "user@gmail.com";
            const string Password = "_Aa123456";
            if (await roleManager.FindByNameAsync(Role.Admin) == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(Role.Admin));
            }
            if (await roleManager.FindByNameAsync(Role.User) == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(Role.User));
            }

            if (await userManager.FindByNameAsync(AdminEmail) == null)
            {
                var admin = new User
                {
                    Email = AdminEmail, 
                    UserName = AdminEmail,
                    EmailConfirmed = true,
                };

                var user = new User
                {
                    Email = UserEmail,
                    UserName = UserEmail,
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(admin, Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Role.Admin);
                }

                result = await userManager.CreateAsync(user, Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Role.User);
                }
            }
        }
    }
}
