using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.BLL.DTO
{
    public class UsersPerRoleDTO
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public IEnumerable<UserInfoDTO> Users { get; set; }
    }
}
