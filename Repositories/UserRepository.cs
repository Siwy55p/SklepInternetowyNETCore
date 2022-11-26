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
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            user.Adress1rozliczeniowy = _context.Adress1rozliczeniowy.FirstOrDefault(x => x.Adres1rozliczeniowyId == user.Adress1rozliczeniowyId);
            user.Adress2dostawy = _context.Adress2dostawy.FirstOrDefault(x => x.Adres2dostawyId == user.Adress2dostawyId);
            //List<Order> listaZamowienUzytkownika = _context.Users.Where(u => u.Id == id).Include()  .Include(user => user.User).ToList();
            return user;
        }

        public ApplicationUser UpdateUser(ApplicationUser user)
        {

            var users = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if(users != null)
            {
                _context.Update(users);
                _context.SaveChanges();
            }
            else
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }

            return user;
        }
    }
}
