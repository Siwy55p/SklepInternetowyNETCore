using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class MetodyPlatnosciService : IMetodyPlatnosci
    {
        private readonly ApplicationDbContext _context;

        public MetodyPlatnosciService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MetodyPlatnosci metodyPlatnosci)
        {
            _context.MetodyPlatnosci.Add(metodyPlatnosci);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            MetodyPlatnosci metodaPlatnosci = await _context.MetodyPlatnosci.Where(x=>x.Id == id).FirstOrDefaultAsync();
            _context.Remove(metodaPlatnosci);

        }

        public async Task Update(int id)
        {
            MetodyPlatnosci metodaPlatnosci = await _context.MetodyPlatnosci.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.Update(metodaPlatnosci);
            _context.SaveChanges();
        }

    }
}
