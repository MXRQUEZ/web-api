﻿using DAL.Models;
using DAL.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.ApplicationContext.Configurations
{
    public sealed class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
        {
            builder
                .HasData(
                    new IdentityUserRole<int>
                    {
                        RoleId = 1,
                        UserId = 1
                    },
                    new IdentityUserRole<int>
                    {
                        RoleId = 2,
                        UserId = 2
                    }
                    );

        }
    }
}
