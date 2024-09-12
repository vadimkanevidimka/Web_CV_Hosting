using CVRecognizingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CVRecognizingService.Infrastructure.DataAccess
{
    public class CVRecDBContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<BaseDocument> Documents { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BaseDocument>()
                .ToCollection("Documents");
        }
    }
}
