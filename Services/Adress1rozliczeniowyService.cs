using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class Adress1rozliczeniowyService : IAdress1rozliczeniowyService
    {
        private readonly ApplicationDbContext _context;

        public Adress1rozliczeniowyService(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Delete(int id)
        {
            var adres1rozliczeniowy = _context.Adress1rozliczeniowy.Find(id);
            _context.Adress1rozliczeniowy.Remove(adres1rozliczeniowy);

            _context.SaveChanges();
            return id;
        }

        public Adress1rozliczeniowy Get(string UserId)
        {
            var Adressy1rozliczeniowy = _context.Adress1rozliczeniowy.ToList();
            var adress1Rozliczeniowy = Adressy1rozliczeniowy.Find(x => x.UserID == UserId);
            return adress1Rozliczeniowy;
        }

        public List<Adress1rozliczeniowy> List()
        {
            var Adress1rozliczeniowy = _context.Adress1rozliczeniowy.ToList();
            return Adress1rozliczeniowy;
        }

        public int Save(Adress1rozliczeniowy adress1Rozliczeniowy)
        {
            _context.Adress1rozliczeniowy.Add(adress1Rozliczeniowy);
            int id = _context.SaveChanges();
            return id;
        }

        public Adress1rozliczeniowy Update(Adress1rozliczeniowy adress1Rozliczeniowy)
        {
            var result = _context.Adress1rozliczeniowy.SingleOrDefault(u => u.UserID == adress1Rozliczeniowy.UserID);
            if (result != null)
            {
                result.Ulica = adress1Rozliczeniowy.Ulica;
                result.KodPocztowy = adress1Rozliczeniowy.KodPocztowy;
                result.Miasto = adress1Rozliczeniowy.Miasto;
                result.Kraj = adress1Rozliczeniowy.Kraj;
                result.Telefon = adress1Rozliczeniowy.Telefon;
                _context.SaveChanges();
            }
            _context.Add(adress1Rozliczeniowy);
            return adress1Rozliczeniowy;
        }
    }
}
