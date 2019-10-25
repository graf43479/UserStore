using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using UserStore.BLL.Models;


namespace UserStore.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> CreateAsync(UserDTO userDto, string callbackUrlBase);
        Task<ClaimsIdentity> AuthenticateAsync(UserDTO userDto);
        Task SetInitialData(UserDTO adminDto, List<string> roles);
        IQueryable<UserDTO> GetUsers();
        Task<OperationDetails> IsEmailConfirmedAsync(UserDTO userDto);
        Task<OperationDetails> ConfirmEmailAsync(string userId, string code);
        Task<OperationDetails> ResetPasswordAsync(string email, string callbackUrlBase);
        Task<OperationDetails> DoResetPaswordAsync(ResetPasswordViewModel model);
        Task<int> CheckForAttemptsAsync(string email);

        Task<OperationDetails> CreateExceptionAsync(ExceptionDetailDTO exceptionDetail);
        
        Task<UserDTO> GetUserByNameAsync(string userName);
        Task<OperationDetails> UpdateUserInfoAsync(UserDTO userDTO, string name);
    }
}
