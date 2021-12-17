using System;
using DAL.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.ApplicationContext.Configurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private const string AdminEmail = "admin@gmail.com";
        private const string UserEmail = "user@gmail.com";
        private const string Password = "_Aa123456";
        private readonly PasswordHasher<User> _hasher = new();

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasData(
                    new User
                    {
                        Id = 1,
                        Email = AdminEmail,
                        NormalizedEmail = AdminEmail.ToUpper(),
                        UserName = AdminEmail,
                        NormalizedUserName = AdminEmail.ToUpper(),
                        EmailConfirmed = true,
                        PasswordHash = _hasher.HashPassword(null, Password),
                        SecurityStamp = Guid.NewGuid().ToString()
                    },
                    new User
                    {
                        Id = 2,
                        Email = UserEmail,
                        NormalizedEmail = UserEmail.ToUpper(),
                        UserName = UserEmail,
                        NormalizedUserName = UserEmail.ToUpper(),
                        EmailConfirmed = true,
                        PasswordHash = _hasher.HashPassword(null, Password),
                        SecurityStamp = Guid.NewGuid().ToString()
                    }
                );
        }
    }
}