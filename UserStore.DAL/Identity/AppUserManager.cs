using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;
using UserStore.DAL.EF;
using UserStore.DAL.Entities;

namespace UserStore.DAL.Identity
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store)
        {
        }

     /*   public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            ApplicationContext db = context.Get<ApplicationContext>();
            AppUserManager manager = new AppUserManager(new UserStore<AppUser>(db));

            //изменения в логике работы валидатора, дополнение
            manager.PasswordValidator = new CustomPasswordValidator() //new PasswordValidator()
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true
            };

               //manager.UserValidator = new CustomUserValidator(manager) //new UserValidator<AppUser>(manager)
               //{
               //    AllowOnlyAlphanumericUserNames = true,
               //    RequireUniqueEmail = true
               //};
               

            manager.UserValidator = new CustomUserValidator();

            return manager;
        }*/
    }
}
