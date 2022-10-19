using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class Adress2dostawyService : IAdress2dostawyService
    {
        private readonly ApplicationDbContext _context;

        public Adress2dostawyService(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Delete(int id)
        {
            var adres2dostawy = _context.Adress2dostawy.Find(id);
            _context.Adress2dostawy.Remove(adres2dostawy);

            _context.SaveChanges();
            return id;
        }

        public Adress2dostawy Get(string UserId)
        {
            var Adressy2dostawy = _context.Adress2dostawy.ToList();
            var adress2dostawy = Adressy2dostawy.Find(x => x.UserID == UserId);
            return adress2dostawy;
        }

        public List<Adress2dostawy> List()
        {
            var Adress2dostawy = _context.Adress2dostawy.ToList();
            return Adress2dostawy;
        }

        public int Save(Adress2dostawy adress2dostawy)
        {
            _context.Adress2dostawy.Add(adress2dostawy);
            int id = _context.SaveChanges();
            return id;
        }

        public Adress2dostawy Update(Adress2dostawy adress2dostawy)
        {
            var result = _context.Adress2dostawy.SingleOrDefault(u => u.UserID == adress2dostawy.UserID);
            if (result != null)
            {
                result.Ulica = adress2dostawy.Ulica;
                result.KodPocztowy = adress2dostawy.KodPocztowy;
                result.Miasto = adress2dostawy.Miasto;
                result.Kraj = adress2dostawy.Kraj;
                result.Telefon = adress2dostawy.Telefon;
                _context.SaveChanges();
            }
            _context.Add(adress2dostawy);
            return adress2dostawy;
        }
    }
}
