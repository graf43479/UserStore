using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using Ninject.Modules;
using UserStore.BLL.Interfaces;
using UserStore.BLL.Services;

namespace UserStore.WEB.Infrastructure
{
    public class DependencyModule : NinjectModule
    {
        //private IKernel kernel;

        //public DependensyResolver(IKernel kernel)
        //{
        //    this.kernel = kernel;
        //    AddBindings();
        //}

        //public object GetService(Type serviceType)
        //{
        //    return kernel.TryGet(serviceType);
        //}

        //public IEnumerable<object> GetServices(Type serviceType)
        //{
        //    return kernel.GetAll(serviceType);
        //}

        public override void Load()
        {
            Bind<IServiceCreator>().To<ServiceCreator>();
            Bind<IUserService>().To<UserService>();
            Bind<IRoleService>().To<RoleService>();
            Bind<IProductService>().To<ProductService>();
        }

        //private void AddBindings()
        //{
           
        //}
    }
}