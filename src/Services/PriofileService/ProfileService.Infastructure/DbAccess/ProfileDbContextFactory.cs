using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProfileService.Infastructure.DbAccess
{
    public class ProfileDbContextFactory : IDesignTimeDbContextFactory<ProfileDbContext>
    {
        public ProfileDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProfileDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5430;Database=profiles;Username=admin;Password=admin");

            return new ProfileDbContext(optionsBuilder.Options);
        }
    }
}