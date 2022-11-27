using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IMetodyDostawy
    {
        Task AddAsync(MetodyDostawy metodyDostawy);
        Task Update(int id);
        Task Delete(int id);
    }
}
