using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using partner_aluro.Models;
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
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Adress1rozliczeniowy> Adress1rozliczeniowy { get; set; }
        public DbSet<Adress2dostawy> Adress2dostawy { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<ProfilDzialalnosci> ProfileDzialalnosci { get; set; }


        //musimy nadpisac OnModelCreating(ModelBuild builder) musimy nadpisac metoda ktora pochodzi DbContext
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());

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
