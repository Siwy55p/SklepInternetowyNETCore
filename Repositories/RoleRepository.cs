using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using partner_aluro.Core.Repositories;
using partner_aluro.Data;

namespace partner_aluro.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
