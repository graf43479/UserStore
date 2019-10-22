using Microsoft.AspNet.Identity.EntityFramework;

namespace UserStore.DAL.Entities
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string name) : base(name) 
        {            
        }
    }  
}
