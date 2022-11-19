using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface INewsletter
    {
        void Add(Newsletter newsletter);

        void Delete(int id);

        void Edit(Newsletter newsletter);

        Task<Newsletter> GetAsync(int id);

    }
}
