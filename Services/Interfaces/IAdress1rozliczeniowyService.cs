using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IAdress1rozliczeniowyService
    {
        List<Adress1rozliczeniowy> List();

        Adress1rozliczeniowy Update(Adress1rozliczeniowy adress1Rozliczeniowy);

        int Delete(int id);

        Adress1rozliczeniowy Get(string UserId);

        int Save(Adress1rozliczeniowy adress1Rozliczeniowy);
    }
}
