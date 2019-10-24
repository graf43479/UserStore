using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc.FilterBindingSyntax;
using UserStore.BLL.Interfaces;
using UserStore.BLL.Services;
using UserStore.WEB.Filters;

namespace UserStore.WEB.Infrastructure
{
    public class DependencyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IServiceCreator>().To<ServiceCreator>();
            Bind<IUserService>().To<UserService>();
            Bind<IRoleService>().To<RoleService>();
            Bind<IProductService>().To<ProductService>();
            Bind<IExceptionService>().To<ExceptionService>();
            Bind<IAdminService>().To<AdminService>();
            this.Kernel.BindFilter<ExceptionLoggerAttribute>(FilterScope.Action, 0); //.WhenControllerHas<MyExceptionLogAttribute>
        }      
    }
}