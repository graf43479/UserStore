using System;
using System.Threading.Tasks;
using UserStore.DAL.Entities;
using UserStore.DAL.Identity;
using UserStore.DAL.Repositories;

namespace UserStore.DAL.Interfaces
{
   public interface IUnitOfWork : IDisposable
    {
        AppUserManager UserManager { get; }
        IRepository<ClientProfile> Clients { get; }
        AppRoleManager RoleManager { get; }
        IRepository<Product> Products { get; }
        IRepository<ExceptionDetail> ExceptionDetails { get; }
        Task SaveAsync();
    }
}
