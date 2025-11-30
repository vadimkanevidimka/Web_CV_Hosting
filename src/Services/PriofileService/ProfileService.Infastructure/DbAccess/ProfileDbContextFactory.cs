using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProfileService.Infastructure.DbAccess
{
    public class ProfileDbContextFactory : IDesignTimeDbContextFactory<ProfileDbContext>
    {
        public ProfileDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProfileDbContext>();
            optionsBuilder.UseNpgsql("Host=profilesdb;Port=5432;Database=profiles;Username=admin;Password=admin");

            return new ProfileDbContext(optionsBuilder.Options);
        }
    }
}