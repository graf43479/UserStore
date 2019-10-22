using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using UserStore.BLL.Interfaces;
using UserStore.BLL.Services;

[assembly: OwinStartup(typeof(UserStore.WEB.App_Start.IdentityConfig))]

namespace UserStore.WEB.App_Start
{
    public class IdentityConfig
    {        

        IServiceCreator serviceCreator = new ServiceCreator();

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,                
                LoginPath = new PathString("/Account/Login")
            });
        }

        private IUserService CreateUserService()
        {            
            return serviceCreator.CreateUserService("DefaultConnection");

        }
    }
}