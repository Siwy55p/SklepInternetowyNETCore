using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IMetodyPlatnosci
    {
        Task AddAsync(MetodyPlatnosci metodyPlatnosci);
        Task Update(int id);
        Task Delete(int id);
    }
}
