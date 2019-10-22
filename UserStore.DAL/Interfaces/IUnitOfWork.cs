using System;
using System.Threading.Tasks;
using UserStore.DAL.Entities;
using UserStore.DAL.Identity;

namespace UserStore.DAL.Interfaces
{
   public interface IUnitOfWork : IDisposable
    {
        AppUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        AppRoleManager RoleManager { get; }
        IRepository<Product> Products { get; }
        Task SaveAsync();
    }
}
