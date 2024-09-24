using AuthService.DataAccess.Entities;
using AuthService.DataAccess.Persistans.Configs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataAccess.Persistans.DbContext
{
    public class AuthorizationDbContext : IdentityDbContext<User>
    {
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("identity");

            base.OnModelCreating(builder);
            builder.Entity<RefreshToken>().ToTable("RefreshTokens");

            builder.ApplyConfiguration(new RefreshTokenConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());

        }
    }
}
