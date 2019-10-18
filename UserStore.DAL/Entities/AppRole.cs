using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UserStore.DAL.Entities
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string name) : base(name) 
        {
            
        }
       

        public virtual IEnumerable<Usr> GetUsersInRole()
        {
            return this.Users.Where(x => x.RoleId == this.Id).Select(x => new Usr { RoleId = x.RoleId, UserId = x.UserId });
        }
    }

    public class Usr
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
