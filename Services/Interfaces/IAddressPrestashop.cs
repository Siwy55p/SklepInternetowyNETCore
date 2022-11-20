using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IAddressPrestashop
    {
        void Add(AddresPrestashop address);

        AddresPrestashop Get(int id);

    }
}
