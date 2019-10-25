using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;
using UserStore.DAL.EF;
using System;
using System.Collections.Generic;

namespace UserStore.DAL.Repositories
{
    public class ClientRepository : IRepository<ClientProfile> //IClientManager
    {        
                
        private ApplicationContext Database { get; set; } 

        public ClientRepository(ApplicationContext context)
        {
            Database = context;
        }
        public void Create(ClientProfile item)
        {
            Database.ClientProfiles.Add(item);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<ClientProfile> GetAll()
        {
            throw new NotImplementedException();
        }

        public ClientProfile Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ClientProfile> Find(Func<ClientProfile, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Update(ClientProfile item)
        {
            throw new NotImplementedException();
        }

        public void Delete(ClientProfile item)
        {
            throw new NotImplementedException();
        }
    }
}
