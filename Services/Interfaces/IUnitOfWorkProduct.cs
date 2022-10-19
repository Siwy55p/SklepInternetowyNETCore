using partner_aluro.Core.Repositories;
using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IUnitOfWorkProduct
    {
        IProductService Product { get; }

        ICategoryService Category { get; }
    }
}
