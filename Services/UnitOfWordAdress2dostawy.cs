using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class UnitOfWordAdress2dostawy : IUnitOfWorkAdress2dostawy
    {
        public IAdress2dostawyService adress2dostawy {get;}

        public UnitOfWordAdress2dostawy(IAdress2dostawyService _adress2dostawy)
        {
            adress2dostawy = _adress2dostawy;
        }
    }
}
