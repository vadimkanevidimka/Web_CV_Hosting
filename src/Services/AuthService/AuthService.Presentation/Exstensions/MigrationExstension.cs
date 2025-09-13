using AuthService.DataAccess.Persistans.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Presentation.Exstensions
{
    public static class MigrationExstension
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using AuthorizationDbContext context = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();

            context.Database.Migrate();
        }
    }
}
