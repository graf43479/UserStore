using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UserStore.BLL.DTO;

namespace UserStore.WEB.Models
{
    public class UsersPerRoleViewModel
    {
        public IEnumerable<UsersPerRoleDTO> UsersPerRole { get; set; }
    }

  

   
}