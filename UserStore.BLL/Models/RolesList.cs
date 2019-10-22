using System.Collections.Generic;
using UserStore.DAL.Entities;

namespace UserStore.BLL.Models
{
    public class RolesList
    {
        IEnumerable<AppRole> Roles { get; }
    }
}
