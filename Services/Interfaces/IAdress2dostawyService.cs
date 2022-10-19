using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IAdress2dostawyService
    {
        List<Adress2dostawy> List();

        Adress2dostawy Update(Adress2dostawy adress2dostawy);

        int Delete(int id);

        Adress2dostawy Get(string UserId);

        int Save(Adress2dostawy adress2dostawy);
    }
}
