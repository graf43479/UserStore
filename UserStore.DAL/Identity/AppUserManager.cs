using Microsoft.AspNet.Identity;
using System;
using UserStore.DAL.Entities;

namespace UserStore.DAL.Identity
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store)
        {           
            DefaultAccountLockoutTimeSpan = TimeSpan.FromHours(1);
            MaxFailedAccessAttemptsBeforeLockout = int.MaxValue;
        }           
    }
}
