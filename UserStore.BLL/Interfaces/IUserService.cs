using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using UserStore.BLL.Models;
using UserStore.DAL.Entities;

namespace UserStore.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto, string callbackUrlBase);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task SetInitialData(UserDTO adminDto, List<string> roles);

        IQueryable<UserDTO> GetUsers();
        Task<OperationDetails> IsEmailConfirmedAsync(UserDTO userDto);
        Task<OperationDetails> ConfirmEmailAsync(string userId, string code);

        Task<OperationDetails> ResetPasswordAsync(string email, string callbackUrlBase);
        Task<OperationDetails> DoResetPaswordAsync(ResetPasswordViewModel model);
    }
}
