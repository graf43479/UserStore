using System.Collections.Generic;
using UserStore.BLL.DTO;


namespace UserStore.BLL.Models
{
    public class RoleEditModel
    {        
        public string roleId { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserInfoDTO> Members { get; set; }
        public IEnumerable<UserInfoDTO> NonMembers { get; set; }
    }
}
