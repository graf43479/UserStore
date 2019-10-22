using Ninject.Modules;
using UserStore.BLL.Interfaces;
using UserStore.BLL.Services;
using UserStore.DAL.Interfaces;
using UserStore.DAL.Repositories;

namespace UserStore.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule(string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<IdentityUnitOfWork>().WithConstructorArgument(connectionString);
            Bind<IServiceCreator>().To<ServiceCreator>().WithConstructorArgument(connectionString);
        }
    }
}
