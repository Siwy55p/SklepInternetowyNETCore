using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using partner_aluro.ViewComponents;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace partner_aluro.Services
{
    public class ContactPrestashopService : IContactPrestashop
    {
        private readonly ApplicationDbContext _context;

        public ContactPrestashopService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ContactPrestashop contact)
        {
            _context.ContactsPrestashop.Add(contact);
            _context.SaveChanges();
        }

        public ContactPrestashop Get(int id)
        {
            ContactPrestashop person = _context.ContactsPrestashop.Where(x => x.Id == id).FirstOrDefault();
            return person;
        }
    }
}
