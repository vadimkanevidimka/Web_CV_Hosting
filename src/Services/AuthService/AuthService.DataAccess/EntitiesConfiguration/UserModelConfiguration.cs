using AuthService.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.DataAccess.EntitiesConfiguration
{
    internal class UserModelConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            builder.HasIndex(u => u.NormalizedEmail)
                .HasDatabaseName("EmailIndex")
                .IsUnique();

            // Уникальность UserName
            builder.HasIndex(u => u.NormalizedUserName)
                .HasDatabaseName("UserNameIndex")
                .IsUnique();

            builder.Property(u => u.NormalizedEmail)
                .HasMaxLength(128);

            // Ограничение на длину полей
            builder.Property(u => u.UserName)
                .HasMaxLength(256);
            builder.Property(u => u.NormalizedUserName)
                .HasMaxLength(256);
            builder.Property(u => u.Email)
                .HasMaxLength(256);
            builder.Property(u => u.NormalizedEmail)
                .HasMaxLength(256);

            // Обязательные поля
            builder.Property(u => u.EmailConfirmed)
                .IsRequired();
            builder.Property(u => u.PhoneNumberConfirmed)
                .IsRequired();
            builder.Property(u => u.TwoFactorEnabled)
                .IsRequired();
            builder.Property(u => u.LockoutEnabled)
                .IsRequired();
            builder.Property(u => u.AccessFailedCount)
                .IsRequired();

            // Столбцы с индексами
            builder.HasIndex(u => u.SecurityStamp)
                .HasDatabaseName("identity");
        }
    }
}
