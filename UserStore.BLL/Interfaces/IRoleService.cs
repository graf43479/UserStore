using System.Collections.Generic;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using UserStore.BLL.Models;

namespace UserStore.BLL.Interfaces
{
    public interface IRoleService
    {
        Task<OperationDetails> CreateAsync(string roleName);
        Task<OperationDetails> DeleteAsync(string roleId);
        Task<OperationDetails> UpdateAsync(RoleDTO roleDto);
        Task<RoleEditModel> GetRoleEditModelAsync(string roleId);
        IEnumerable<UsersPerRoleDTO> GetUsersPerRole(string roleId, string userId);
        Task<OperationDetails> RemoveFromRoleAsync(string roleName, string[] idsToDelete);
        Task<OperationDetails> AddToRoleAsync(string roleName, string[] idsToAdd);
    }
}
