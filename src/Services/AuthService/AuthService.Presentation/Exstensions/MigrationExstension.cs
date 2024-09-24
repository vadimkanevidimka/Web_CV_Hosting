using AuthService.DataAccess.Persistans.DbContext;
using Microsoft.EntityFrameworkCore;

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
