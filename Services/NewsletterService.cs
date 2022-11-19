using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class NewsletterService : INewsletter
    {
        ApplicationDbContext _context;

        public NewsletterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Newsletter newsletter)
        {
            _context.Add<Newsletter>(newsletter);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            Newsletter newsletter = _context.Newsletter.Where(x => x.NewsletterID == id).Include(x => x.listaEmail).FirstOrDefault();
            _context.Remove(newsletter);
            _context.SaveChanges();
        }

        public void Edit(Newsletter newsletter)
        {
            _context.Newsletter.Update(newsletter);
            _context.SaveChanges();
        }

        public async Task<Newsletter> GetAsync(int id)
        {
            Newsletter newsletter =  await _context.Newsletter.Where(x => x.NewsletterID == id).Include(x => x.listaEmail).FirstOrDefaultAsync();

            return newsletter;
        }
    }
}
