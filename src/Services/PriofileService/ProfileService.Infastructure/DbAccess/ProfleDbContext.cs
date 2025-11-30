using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.ApplicantProfile;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProfileService.Infastructure.DbAccess;

public class ProfileDbContext : DbContext
{
    // DbSet для основной сущности профиля соискателя
    public DbSet<ApplicantProfile> ApplicantProfiles { get; set; }

    // DbSet для всех вложенных сущностей
    public DbSet<Education> Educations { get; set; }
    public DbSet<WorkExperience> WorkExperiences { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Citizenship> Citizenships { get; set; }
    public DbSet<DriverLicense> DriverLicenses { get; set; }
    public DbSet<PortfolioItem> PortfolioItems { get; set; }
    public DbSet<SalaryExpectations> SalaryExpectations { get; set; }
    public DbSet<EmploymentType> EmploymentTypes { get; set; }

    public ProfileDbContext(DbContextOptions<ProfileDbContext> options)
        : base(options)
    {
    }

    public ProfileDbContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure DateTime to UTC converter
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(nullableDateTimeConverter);
                }
            }
        }

        modelBuilder.Entity<ApplicantProfile>()
            .HasMany(p => p.Educations)
            .WithOne(e => e.ApplicantProfile)
            .HasForeignKey(e => e.ApplicantProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicantProfile>()
            .HasMany(p => p.WorkExperiences)
            .WithOne(w => w.ApplicantProfile)
            .HasForeignKey(w => w.ApplicantProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicantProfile>()
            .HasMany(p => p.Skills)
            .WithOne(s => s.ApplicantProfile)
            .HasForeignKey(s => s.ApplicantProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicantProfile>()
            .HasMany(p => p.Languages)
            .WithOne(l => l.ApplicantProfile)
            .HasForeignKey(l => l.ApplicantProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicantProfile>()
            .HasMany(p => p.Citizenships)
            .WithOne(c => c.ApplicantProfile)
            .HasForeignKey(c => c.ApplicantProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicantProfile>()
            .HasMany(p => p.DriverLicenses)
            .WithOne(d => d.ApplicantProfile)
            .HasForeignKey(d => d.ApplicantProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicantProfile>()
            .HasMany(p => p.PortfolioItems)
            .WithOne(pi => pi.ApplicantProfile)
            .HasForeignKey(pi => pi.ApplicantProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicantProfile>()
            .HasOne(p => p.SalaryExpectations)
            .WithOne(s => s.ApplicantProfile)
            .HasForeignKey<SalaryExpectations>(s => s.ApplicantProfileId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicantProfile>()
            .HasOne(p => p.EmploymentType)
            .WithOne(e => e.ApplicantProfile)
            .HasForeignKey<EmploymentType>(e => e.ApplicantProfileId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}