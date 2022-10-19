using partner_aluro.Core.Repositories;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class UnitOfWorkCategory : IUnitOfWorkCategory
    {
        public ICategoryService Category { get; }


        public UnitOfWorkCategory(ICategoryService category)
        {
            Category = category;
        }
    }
}
