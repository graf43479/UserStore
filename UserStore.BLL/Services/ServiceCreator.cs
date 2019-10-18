using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.Interfaces;
using UserStore.DAL.Repositories;

namespace UserStore.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IProductService CreateProductService(string connection)
        {
            return new ProductService(new IdentityUnitOfWork(connection));
        }

        public IRoleService CreateRoleService(string connection)
        {
            return new RoleService(new IdentityUnitOfWork(connection));
        }

        public IUserService CreateUserService(string connection)
        {
            return new UserService(new IdentityUnitOfWork(connection));
        }
    }
}
