using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.DAL.Entities;

namespace UserStore.BLL.Models
{
    public class RoleEditModel
    {
        //public AppRole Role { get; set; }
        public string roleId { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserInfoDTO> Members { get; set; }
        public IEnumerable<UserInfoDTO> NonMembers { get; set; }
    }
}
