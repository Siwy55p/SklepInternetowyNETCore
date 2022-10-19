using Microsoft.AspNetCore.Mvc;

namespace partner_aluro.Core.Repositories
{
    //
    public interface IUnitOfWork
    {
        IUserRepository User { get; }

        IRoleRepository Role { get; }


    }
}
