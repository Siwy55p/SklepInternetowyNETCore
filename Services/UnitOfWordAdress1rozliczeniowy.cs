using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class UnitOfWordAdress1rozliczeniowy : IUnitOfWorkAdress1rozliczeniowy
    {
        public IAdress1rozliczeniowyService adress1Rozliczeniowy {get;}

        public UnitOfWordAdress1rozliczeniowy(IAdress1rozliczeniowyService _adress1Rozliczeniowy)
        {
            adress1Rozliczeniowy = _adress1Rozliczeniowy;
        }
    }
}
