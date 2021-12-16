using DAL.Models;
using DAL.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.ApplicationContext.Configurations
{
    public sealed class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
        {
            builder
                .HasData(
                    new IdentityRole<int>
                    {
                        Id = 1,
                        Name = Role.Admin,
                        NormalizedName = Role.Admin.ToUpper()
                    },
                    new IdentityRole<int>
                    {
                        Id = 2,
                        Name = Role.User,
                        NormalizedName = Role.User.ToUpper()
                    }
                );
        }
    }
}
