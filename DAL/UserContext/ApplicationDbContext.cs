using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.UserContext
{
    public sealed class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasIndex(p => new {p.Name, p.Platform, p.DateCreated, p.TotalRating})
                .HasFilter("[Name] IS NOT NULL")
                .HasFilter("[DateCreated] IS NOT NULL");
            modelBuilder.Entity<Product>()
                .HasData(
                    new Product
                    {
                        Id = 1,
                        Name = "PC Product1",
                        Platform = (int) Platforms.PersonalComputer,
                        DateCreated = "28/03/2019",
                        TotalRating = 5
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "PC Product2",
                        Platform = (int) Platforms.PersonalComputer,
                        DateCreated = "28/03/2020",
                        TotalRating = 4
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "PC Product3",
                        Platform = (int) Platforms.PersonalComputer,
                        DateCreated = "28/03/2021",
                        TotalRating = 3
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "PC Product4",
                        Platform = (int) Platforms.PersonalComputer,
                        DateCreated = "28/03/2020",
                        TotalRating = 5
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "Mobile Product1",
                        Platform = (int) Platforms.Mobile,
                        DateCreated = "28/03/2018",
                        TotalRating = 3
                    },
                    new Product
                    {
                        Id = 6,
                        Name = "Mobile Product2",
                        Platform = (int) Platforms.Mobile,
                        DateCreated = "28/03/2021",
                        TotalRating = 5
                    },
                    new Product
                    {
                        Id = 7,
                        Name = "Mobile Product3",
                        Platform = (int) Platforms.Mobile,
                        DateCreated = "28/03/2018",
                        TotalRating = 4
                    },
                    new Product
                    {
                        Id = 8,
                        Name = "PS Product1",
                        Platform = (int) Platforms.PlayStation,
                        DateCreated = "28/03/2021",
                        TotalRating = 5
                    },
                    new Product
                    {
                        Id = 9,
                        Name = "PS Product2",
                        Platform = (int) Platforms.PlayStation,
                        DateCreated = "28/03/2021",
                        TotalRating = 4
                    },
                    new Product
                    {
                        Id = 10,
                        Name = "PS Product3",
                        Platform = (int) Platforms.PlayStation,
                        DateCreated = "28/03/2018",
                        TotalRating = 3
                    },
                    new Product
                    {
                        Id = 11,
                        Name = "Xbox Product1",
                        Platform = (int) Platforms.Xbox,
                        DateCreated = "28/03/2021",
                        TotalRating = 5
                    },
                    new Product
                    {
                        Id = 12, Name = "Xbox Product2",
                        Platform = (int) Platforms.Xbox,
                        DateCreated = "28/03/2019",
                        TotalRating = 4
                    },
                    new Product
                    {
                        Id = 13,
                        Name = "Nintendo Product1",
                        Platform = (int) Platforms.Nintendo,
                        DateCreated = "28/03/2018",
                        TotalRating = 4
                    });
            base.OnModelCreating(modelBuilder);
        }
    }
}