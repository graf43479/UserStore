using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.DAL.Entities;

namespace UserStore.BLL.DTO
{
    public class RoleDTO //: AppRole
    {
        
            public string Id { get; set; }
            public string Name { get; set; }
            public ICollection<UserDTO> Users { get; }

        //this.Id string
        //this.Name sstring
        //this.Users //Icollection<TUserRole>
    
    }
}
