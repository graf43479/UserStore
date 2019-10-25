using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using UserStore.DAL.EF;
using UserStore.DAL.Entities;
using UserStore.DAL.Identity;
using UserStore.DAL.Interfaces;

namespace UserStore.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;

        private AppUserManager userManager;
        private AppRoleManager roleManager;
        private ClientRepository clientRepository;


        private ProductRepository productRepository;
        private ExceptionRepository exceptionRepository;

        public IdentityUnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            userManager = new AppUserManager(new UserStore<AppUser>(db));          
            roleManager = new AppRoleManager(new RoleStore<AppRole>(db));
            clientRepository = new ClientRepository(db);
            productRepository = new ProductRepository(db);
            exceptionRepository = new ExceptionRepository(db);
        }

        public AppUserManager UserManager => userManager;

        public IRepository<ClientProfile> Clients
        {
            get
            {
                if (clientRepository == null)
                    clientRepository = new ClientRepository(db);
                return clientRepository;
            }
        }
    
        public AppRoleManager RoleManager => roleManager;

        public IRepository<Product> Products
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(db);
                return productRepository;
            }
        }

        public IRepository<ExceptionDetail> ExceptionDetails
        {
            get
            {
                if (exceptionRepository == null)
                    exceptionRepository = new ExceptionRepository(db);
                return exceptionRepository;
            }
        }

        

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }


        public void Dispose()
        {
            db.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    clientRepository.Dispose();
                }
                disposed = true;
            }
        }        
    }
}
