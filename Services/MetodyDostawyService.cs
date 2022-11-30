using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class MetodyDostawyService : IMetodyDostawy
    {
        private readonly ApplicationDbContext _context;

        public MetodyDostawyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MetodyDostawy metodyDostawy)
        {
            _context.MetodyDostawy.Add(metodyDostawy);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            MetodyDostawy metodaDostawy = _context.MetodyDostawy.Where(x=>x.Id == id).FirstOrDefault();
            _context.MetodyDostawy.Remove(metodaDostawy);
            _context.SaveChanges();


        }

        public async Task Update(int id)
        {
            MetodyDostawy metodaDostawy = await _context.MetodyDostawy.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.Update(metodaDostawy);
            _context.SaveChanges();
        }

    }
}
