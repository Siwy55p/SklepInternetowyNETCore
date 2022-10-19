using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Core.Repositories;
using partner_aluro.Data;
using partner_aluro.Models;

namespace partner_aluro.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<ApplicationUser> GetUsers()
        {
            return _context.Users.ToList();
        }

        public ApplicationUser GetUser(string id)
        {

            //List<Order> listaZamowienUzytkownika = _context.Users.Where(u => u.Id == id).Include()  .Include(user => user.User).ToList();
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public ApplicationUser UpdateUser(ApplicationUser user)
        {
            _context.Update(user);
            _context.SaveChanges();

            return user;
        }
    }
}
