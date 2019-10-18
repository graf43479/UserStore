using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using UserStore.BLL.Models;

namespace UserStore.BLL.Interfaces
{
    public interface IRoleService
    {
        Task<OperationDetails> Create(string roleName);
        Task<OperationDetails> Delete(string roleId);

        Task<OperationDetails> Update(RoleDTO roleDto);

        Task<RoleEditModel> GetRoleEditModel(string roleId);

        //IEnumerable<IdentityUserRole> GetCustomIdentityUser(string role);

        IEnumerable<UsersPerRoleDTO> GetUsersPerRole(string roleId, string userId);
        Task<OperationDetails> RemoveFromRoleAsync(string roleName, string[] idsToDelete);
        Task<OperationDetails> AddToRoleAsync(string roleName, string[] idsToAdd);
        //IQueryable<RoleDTO> GetRoles();
        //UsersPerRoleDTO GetUsersForRole(string v);

    }
}
