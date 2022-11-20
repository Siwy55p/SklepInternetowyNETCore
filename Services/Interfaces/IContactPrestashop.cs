using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IContactPrestashop
    {
        void Add(ContactPrestashop contact);

        ContactPrestashop Get(int id);

    }
}
