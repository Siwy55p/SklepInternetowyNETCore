using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using partner_aluro.Models;


namespace partner_aluro.Data
{
    public class DBInitializer
    {
        private readonly ApplicationDbContext _context;

        public DBInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SeedProduktData(ApplicationDbContext context)
        {
            //var category = new List<Category>
            //{
            //    new Category(){CategoryId=1,Name="Meble", Description="Donice", NazwaPlikuIkony="donice.jpg" },
            //    new Category(){CategoryId=2,Name="Oświetlenie", Description="Donice", NazwaPlikuIkony="donice.jpg" },
            //    new Category(){CategoryId=3,Name="Donice i Osłonki", Description="Donice", NazwaPlikuIkony="donice.jpg" },
            //    new Category(){CategoryId=4,Name="Dodatki do Wnętrz", Description="Donice", NazwaPlikuIkony="donice.jpg" },
            //    new Category(){CategoryId=5,Name="New Collection", Description="Donice", NazwaPlikuIkony="donice.jpg" },
            //};

            //category.ForEach(k => context.Category.Add(k));
            context.SaveChanges();

            var produkty = new List<Product>
            {
                new Product { Name="Donica1", Description="Opis Donica1", CenaProduktuBrutto=283, DataDodania=DateTime.Now, Bestseller= true},
                new Product { Name="Donica2", Description="Opis Donica2", CenaProduktuBrutto=20, DataDodania=DateTime.Now, Bestseller= true},
                new Product { Name="Donica3", Description="Opis Donica3", CenaProduktuBrutto=200, DataDodania=DateTime.Now, Bestseller= true},
                new Product { Name="Donica4", Description="Opis Donica4", CenaProduktuBrutto=90, DataDodania=DateTime.Now, Bestseller= true},
                new Product { Name="Donica5", Description="Opis Donica5", CenaProduktuBrutto=70, DataDodania=DateTime.Now, Bestseller= true},
            };

            produkty.ForEach(k=>context.Products.Add(k));
            context.SaveChanges();


        }
    }
    
}
