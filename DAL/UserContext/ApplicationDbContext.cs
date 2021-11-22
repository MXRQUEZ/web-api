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
                .HasIndex(p => new { p.Name, p.Platform, p.DateCreated, p.TotalRating, p.Genre, p.Rating, p.Price })
                .HasFilter("[Name] IS NOT NULL")
                .HasFilter("[DateCreated] IS NOT NULL")
                .HasFilter("[Genre] IS NOT NULL")
                .HasFilter("[Price] IS NOT NULL");
            modelBuilder.Entity<Product>()
                .HasData(
                    new Product
                    {
                        Id = 1,
                        Name = "PC Product1",
                        Platform = (int) Platforms.PersonalComputer,
                        DateCreated = "28/03/2019",
                        TotalRating = 5,
                        Genre = "Action",
                        Logo = "https://res.cloudinary.com/mxrquez/image/upload/v1637149584/logo1_oefz4s.jpg",
                        Background = "https://res.cloudinary.com/mxrquez/image/upload/v1637149583/background1_zmonut.jpg",
                        Count = 1,
                        Price = "200$",
                        Rating = (int) Ratings.EighteenPlus
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "PC Product2",
                        Platform = (int) Platforms.PersonalComputer,
                        DateCreated = "28/03/2020",
                        TotalRating = 4,
                        Genre = "Shooter",
                        Logo = "https://res.cloudinary.com/mxrquez/image/upload/v1637149584/logo2_utcyoi.jpg",
                        Background = "https://res.cloudinary.com/mxrquez/image/upload/v1637149583/background2_cv26wh.jpg",
                        Count = 1,
                        Price = "100$",
                        Rating = (int) Ratings.TwelvePlus
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "PC Product3",
                        Platform = (int) Platforms.PersonalComputer,
                        DateCreated = "28/03/2021",
                        TotalRating = 3,
                        Genre = "Shooter",
                        Logo = "https://res.cloudinary.com/mxrquez/image/upload/v1637149585/logo3_idvylc.jpg",
                        Background = "https://res.cloudinary.com/mxrquez/image/upload/v1637149583/background3_s58qsc.jpg",
                        Count = 3,
                        Price = "50$",
                        Rating = (int)Ratings.SixPlus
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "Mobile Product1",
                        Platform = (int) Platforms.Mobile,
                        DateCreated = "28/03/2018",
                        TotalRating = 3,
                        Genre = "Strategy",
                        Logo = "https://res.cloudinary.com/mxrquez/image/upload/v1637149587/logo4_avqczq.jpg",
                        Background = "https://res.cloudinary.com/mxrquez/image/upload/v1637149583/background4_n7epnc.jpg",
                        Count = 2,
                        Price = "10$",
                        Rating = (int)Ratings.All
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "Mobile Product2",
                        Platform = (int) Platforms.Mobile,
                        DateCreated = "28/03/2021",
                        TotalRating = 5,
                        Genre = "Action",
                        Logo = "https://res.cloudinary.com/mxrquez/image/upload/v1637149585/logo5_lmvhi1.jpg",
                        Background = "https://res.cloudinary.com/mxrquez/image/upload/v1637149586/background5_cilkad.jpg",
                        Count = 1,
                        Price = "20$",
                        Rating = (int)Ratings.TwelvePlus
                    },
                    new Product
                    {
                        Id = 6,
                        Name = "PS Product1",
                        Platform = (int) Platforms.PlayStation,
                        DateCreated = "28/03/2021",
                        TotalRating = 5,
                        Genre = "Shooter",
                        Logo = "https://res.cloudinary.com/mxrquez/image/upload/v1637149587/logo6_tjbwjn.jpg",
                        Background = "https://res.cloudinary.com/mxrquez/image/upload/v1637149583/background6_shsn1b.jpg",
                        Count = 1,
                        Price = "300$",
                        Rating = (int)Ratings.TwelvePlus
                    },
                    new Product
                    {
                        Id = 7,
                        Name = "PS Product2",
                        Platform = (int) Platforms.PlayStation,
                        DateCreated = "28/03/2021",
                        TotalRating = 4,
                        Genre = "Casual",
                        Logo = "https://res.cloudinary.com/mxrquez/image/upload/v1637149584/logo1_oefz4s.jpg",
                        Background = "https://res.cloudinary.com/mxrquez/image/upload/v1637149583/background1_zmonut.jpg",
                        Count = 2,
                        Price = "200$",
                        Rating = (int)Ratings.TwelvePlus
                    },
                    new Product
                    {
                        Id = 8,
                        Name = "Xbox Product1",
                        Platform = (int) Platforms.Xbox,
                        DateCreated = "28/03/2021",
                        TotalRating = 5,
                        Genre = "Shooter",
                        Logo = "https://res.cloudinary.com/mxrquez/image/upload/v1637149594/logo8_c296hm.jpg",
                        Background = "https://res.cloudinary.com/mxrquez/image/upload/v1637149584/background8_mqb9le.jpg",
                        Count = 1,
                        Price = "200$",
                        Rating = (int)Ratings.EighteenPlus
                    },
                    new Product
                    {
                        Id = 9,
                        Name = "Nintendo Product1",
                        Platform = (int) Platforms.Nintendo,
                        DateCreated = "28/03/2018",
                        TotalRating = 4,
                        Genre = "Racing",
                        Logo = "https://res.cloudinary.com/mxrquez/image/upload/v1637149587/logo9_k894ri.jpg",
                        Background = "https://res.cloudinary.com/mxrquez/image/upload/v1637149584/background9_f9fsd8.jpg",
                        Count = 4,
                        Price = "50$",
                        Rating = (int)Ratings.All
                    });
            base.OnModelCreating(modelBuilder);
        }
    }
}