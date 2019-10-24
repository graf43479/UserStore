using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;

namespace UserStore.BLL.Interfaces
{
    public interface IAdminService
    {
        Task<IQueryable<UserDTO>> GetUsersAsync();

        Task<IEnumerable<ExceptionDetailDTO>> GetLogAsync();
        Task<OperationDetails> DeleteExceptionDetailByIDAsync(int id);
        Task<OperationDetails> DeleteExceptionDetailAsync();
        Task<OperationDetails> DeleteUserAsync(string id);
    }
}
