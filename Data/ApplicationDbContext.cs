﻿using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using partner_aluro.Models;
using System.Reflection.Emit;
using static NuGet.Packaging.PackagingConstants;
//klasa do do polaczenia sie do bazy z EntityFramework https://learn.microsoft.com/en-us/aspnet/web-api/overview/testing-and-debugging/mocking-entity-framework-when-unit-testing-aspnet-web-api-2
namespace partner_aluro.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        //Musimy zdefiniowac nasze modele
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }


        //Tutaj bedzie BdSet 
        //Sluzy do tego ktory model odpowiada ktorej encji w bazie danych DbSet 
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Adress1rozliczeniowy> Adress1rozliczeniowy { get; set; }
        public DbSet<Adress2dostawy> Adress2dostawy { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<MetodyDostawy> MetodyDostawy { get; set; }
        public DbSet<MetodyPlatnosci> MetodyPlatnosci { get; set; }
        public DbSet<ProfilDzialalnosci> ProfileDzialalnosci { get; set; }
        public DbSet<Newsletter> Newsletter { get; set; }

        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<ContactPrestashop> ContactsPrestashop { get; set; }
        public DbSet<AddresPrestashop> AddressPrestashop { get; set; }
        public DbSet<ProductPrestashop> ProductsPrestashop { get; set; }
        public DbSet<ProductNazwyPrestashop> ProductsNamePrestashop { get; set; }
        public DbSet<ProductQuantityPrestashop> ProductsQuantityPrestashop { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<SMS> SMS { get; set; }

        //musimy nadpisac OnModelCreating(ModelBuild builder) musimy nadpisac metoda ktora pochodzi DbContext
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());

            //builder.Entity<Category>()
            //    .ToTable("Category")
            //    .HasDiscriminator<int>("CategoryId")
            //    .HasValue<SubCat>(1);


        }
    }

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.Imie).HasMaxLength(255);
            builder.Property(u => u.Nazwisko).HasMaxLength(255);
        }
    }

}
