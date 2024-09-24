using AuthService.DataAccess.Entities;
using AuthService.DataAccess.Persistans.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataAccess.Persistans;

public static class DbInitializer
{
    public static void Initialize(AuthorizationDbContext appDbcontext)
    {
        appDbcontext.Database.Migrate();
    }

    public static void Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        var adminRole = roleManager.FindByNameAsync(Roles.Admin).Result;
        if (adminRole is null)
        {
            adminRole = new IdentityRole()
            {
                Name = Roles.Admin
            };
            roleManager.CreateAsync(adminRole).Wait();

            var userRole = new IdentityRole()
            {
                Name = Roles.User
            };
            roleManager.CreateAsync(userRole).Wait();

            var admin = new User
            {
                Email = "admin",
                UserName = "admin",
                EmailConfirmed = true
            };
            var result = userManager.CreateAsync(admin, "admin").Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(admin, Roles.Admin).Wait();
            }
        }
    }
}