using System;
using System.Collections.Generic;
using System.Text;
using UserStore.DAL.Entities;

namespace UserStore.DAL.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(ClientProfile item);
    }
}
