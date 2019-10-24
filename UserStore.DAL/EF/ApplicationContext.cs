using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using UserStore.DAL.Entities;
using UserStore.DAL.Identity;
using Microsoft.AspNet.Identity;
using System.Data.Entity.ModelConfiguration.Conventions;
using UserStore.DAL.Configurations;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity.Owin;

namespace UserStore.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<AppUser>
    {
        public ApplicationContext(string connectionString) : base(connectionString)        {     }

        static ApplicationContext()
        {
            Database.SetInitializer<ApplicationContext>(new IdentityDbInit());
        }

        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<ExceptionDetail> ExceptionDetails { get; set; }
        public DbSet<Product> Products { get; set; }


      /*  protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //конфиги
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Configurations.Add(new ProductConfig());
            modelBuilder.Configurations.Add(new ExceptionDetailConfig());

        }
        */

        internal void Dispose(bool v)
        {
            base.Dispose(v);                
        }
    }

    //public class IdentityDbInit : NullDatabaseInitializer<ApplicationContext>{ } //DropCreateDatabaseAlways<AppIdentityDbContext
    public class IdentityDbInit : DropCreateDatabaseIfModelChanges<ApplicationContext> //DropCreateDatabaseAlways<ApplicationContext>
    {        
            protected override void Seed(ApplicationContext context)
            {
                PerformInitialSetup(context);
                base.Seed(context);
            }

            private void PerformInitialSetup(ApplicationContext context)
            {
                // настройки конфигурации контекста будут указываться здесь
                var provider = new DpapiDataProtectionProvider("SampleAppName");

                //Рождение первого админа
                AppUserManager userMng = new AppUserManager(new UserStore<AppUser>(context));
                AppRoleManager roleMng = new AppRoleManager(new RoleStore<AppRole>(context));
                userMng.UserTokenProvider = new DataProtectorTokenProvider<AppUser>(provider.Create("SampleTokenName"));

            string roleName = "admin";
                string userName = "graf43479";
                string pass = "mypassword";
                string email = "admin@ya.ru";

                //делается синхронно, так как важна последовательность
                string[] otherroles = { "user", "manager", "seo" };
                foreach (string role in otherroles)
                {
                    if (!roleMng.RoleExists(role))
                    {
                        roleMng.Create(new AppRole(role));
                    }
                }

                if (!roleMng.RoleExists(roleName))
                {
                    roleMng.Create(new AppRole(roleName));
                }

                AppUser user = userMng.FindByName(userName);
                if (user == null)
                {
                    userMng.Create(new AppUser{ UserName = userName, Email = email}, pass);
                    user = userMng.FindByName(userName);
                }

                if (!userMng.IsInRole(user.Id, roleName))
                {
                    userMng.AddToRole(user.Id, roleName);
                }
            }
            
    }
}
