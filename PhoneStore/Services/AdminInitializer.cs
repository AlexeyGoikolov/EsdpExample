using Microsoft.AspNetCore.Identity;
using PhoneStore.Models;

namespace PhoneStore.Services;

public class AdminInitializer
{
    public static async Task SeedAdminUser(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string adminEmail = "admin@admin.com";
        string password = "Qwerty123@";

        var roles = new[] { "admin", "user" };
        foreach (var role in roles)
        {
            if (await roleManager.FindByNameAsync(role) is null)
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        if (await userManager.FindByNameAsync(adminEmail) is null)
        {
            User admin = new User { Email = adminEmail, UserName = adminEmail };
            IdentityResult result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "admin");
        }
    }
}