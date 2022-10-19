using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace partner_aluro.Core.Repositories
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
}
