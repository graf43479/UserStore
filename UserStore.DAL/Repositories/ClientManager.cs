using System;
using System.Collections.Generic;
using System.Text;
using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;
using UserStore.DAL.EF;

namespace UserStore.DAL.Repositories
{
    public class ClientManager : IClientManager
    {

        public ApplicationContext Database { get; set; } 

        public ClientManager(ApplicationContext context)
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
    }
}
