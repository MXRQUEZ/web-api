using DAL.Models;
using DAL.Models.Entities;
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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductRating>()
                .HasKey(r => new { r.ProductId, r.UserId });

            modelBuilder.Entity<ProductRating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<ProductRating>()
                .HasOne(r => r.Product)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.ProductId);

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
                        Platform = Platform.PersonalComputer,
                        DateCreated = "28/03/2019",
                        TotalRating = 0,
                        Genre = Genre.Shooter,
                        Logo = "/mxrquez/image/upload/v1637149584/logo1_oefz4s.jpg",
                        Background = "/mxrquez/image/upload/v1637149583/background1_zmonut.jpg",
                        Count = 1,
                        Price = 200,
                        Rating = Rating.EighteenPlus
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "PC Product2",
                        Platform = Platform.PersonalComputer,
                        DateCreated = "28/03/2020",
                        TotalRating = 0,
                        Genre = Genre.Action,
                        Logo = "/mxrquez/image/upload/v1637149584/logo2_utcyoi.jpg",
                        Background = "/mxrquez/image/upload/v1637149583/background2_cv26wh.jpg",
                        Count = 1,
                        Price = 100,
                        Rating = Rating.TwelvePlus
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "PC Product3",
                        Platform = Platform.PersonalComputer,
                        DateCreated = "28/03/2021",
                        TotalRating = 0,
                        Genre = Genre.Casual,
                        Logo = "/mxrquez/image/upload/v1637149585/logo3_idvylc.jpg",
                        Background = "/mxrquez/image/upload/v1637149583/background3_s58qsc.jpg",
                        Count = 3,
                        Price = 50,
                        Rating = Rating.SixPlus
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "Mobile Product1",
                        Platform = Platform.Mobile,
                        DateCreated = "28/03/2018",
                        TotalRating = 0,
                        Genre = Genre.Casual,
                        Logo = "/mxrquez/image/upload/v1637149587/logo4_avqczq.jpg",
                        Background = "/mxrquez/image/upload/v1637149583/background4_n7epnc.jpg",
                        Count = 2,
                        Price = 10,
                        Rating = Rating.All
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "Mobile Product2",
                        Platform = Platform.Mobile,
                        DateCreated = "28/03/2021",
                        TotalRating = 0,
                        Genre = Genre.Fighting,
                        Logo = "/mxrquez/image/upload/v1637149585/logo5_lmvhi1.jpg",
                        Background = "/mxrquez/image/upload/v1637149586/background5_cilkad.jpg",
                        Count = 1,
                        Price = 20,
                        Rating = Rating.TwelvePlus
                    },
                    new Product
                    {
                        Id = 6,
                        Name = "PS Product1",
                        Platform = Platform.PlayStation,
                        DateCreated = "28/03/2021",
                        TotalRating = 0,
                        Genre = Genre.Shooter,
                        Logo = "/mxrquez/image/upload/v1637149587/logo6_tjbwjn.jpg",
                        Background = "/mxrquez/image/upload/v1637149583/background6_shsn1b.jpg",
                        Count = 1,
                        Price = 300,
                        Rating = Rating.TwelvePlus
                    },
                    new Product
                    {
                        Id = 7,
                        Name = "PS Product2",
                        Platform = Platform.PlayStation,
                        DateCreated = "28/03/2021",
                        TotalRating = 0,
                        Genre = Genre.Racing,
                        Logo = "/mxrquez/image/upload/v1637149584/logo1_oefz4s.jpg",
                        Background = "/mxrquez/image/upload/v1637149583/background1_zmonut.jpg",
                        Count = 2,
                        Price = 200,
                        Rating = Rating.TwelvePlus
                    },
                    new Product
                    {
                        Id = 8,
                        Name = "Xbox Product1",
                        Platform = Platform.Xbox,
                        DateCreated = "28/03/2021",
                        TotalRating = 0,
                        Genre = Genre.Racing,
                        Logo = "/mxrquez/image/upload/v1637149594/logo8_c296hm.jpg",
                        Background = "/mxrquez/image/upload/v1637149584/background8_mqb9le.jpg",
                        Count = 1,
                        Price = 200,
                        Rating = Rating.EighteenPlus
                    },
                    new Product
                    {
                        Id = 9,
                        Name = "Nintendo Product1",
                        Platform = Platform.Nintendo,
                        DateCreated = "28/03/2018",
                        TotalRating = 0,
                        Genre = Genre.Racing,
                        Logo = "/mxrquez/image/upload/v1637149587/logo9_k894ri.jpg",
                        Background = "/mxrquez/image/upload/v1637149584/background9_f9fsd8.jpg",
                        Count = 4,
                        Price = 50,
                        Rating = Rating.All
                    });
        }
    }
}