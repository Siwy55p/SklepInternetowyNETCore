using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;


namespace partner_aluro.Services
{
    public class AddressPrestashopService : IAddressPrestashop
    {
        private readonly ApplicationDbContext _context;

        public AddressPrestashopService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(AddresPrestashop address)
        {
            _context.AddressPrestashop.Add(address);
            _context.SaveChanges();
        }

        public AddresPrestashop Get(int id)
        {
            AddresPrestashop address = new AddresPrestashop(); /*_context.AddressPrestashop.Where(x=>x.)*/
            return address;
        }
    }
}
