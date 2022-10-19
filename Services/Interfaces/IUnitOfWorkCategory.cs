using partner_aluro.Core.Repositories;

namespace partner_aluro.Services.Interfaces
{
    public interface IUnitOfWorkCategory
    {
        ICategoryService Category { get; }
    }
}
