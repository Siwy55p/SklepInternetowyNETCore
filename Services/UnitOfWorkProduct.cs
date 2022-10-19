using partner_aluro.Core.Repositories;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class UnitOfWorkProduct : IUnitOfWorkProduct
    {
        public IProductService Product { get; }

        public ICategoryService Category { get; }

        public UnitOfWorkProduct(IProductService product, ICategoryService category)
        {
            Product = product;
            Category = category;
        }
    }
}
