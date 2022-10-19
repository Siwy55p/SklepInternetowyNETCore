using Microsoft.AspNetCore.Mvc;
using partner_aluro.Models;

namespace partner_aluro.Core.Repositories
{
    public interface IUserRepository
    {
        ICollection<ApplicationUser> GetUsers();

        ApplicationUser GetUser(string id);

        ApplicationUser UpdateUser(ApplicationUser user);
    }
}
