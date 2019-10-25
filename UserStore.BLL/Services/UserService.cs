using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.BLL.Identity;
using UserStore.BLL.Infrastructure;
using UserStore.BLL.Interfaces;
using UserStore.BLL.Helpers;
using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using UserStore.BLL.Models;
using System.Threading;
using System;
using AutoMapper;

namespace UserStore.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;            
            Database.UserManager.PasswordValidator = new CustomPasswordValidator();
            Database.UserManager.UserValidator = new CustomUserValidator();
            Database.UserManager.EmailService = new EmailService();
            //Database.UserManager.SmsService

            var provider = new DpapiDataProtectionProvider("YourAppName");
            Database.UserManager.UserTokenProvider = new DataProtectorTokenProvider<AppUser, string>(provider.Create("UserToken"))
    as IUserTokenProvider<AppUser, string>;
        }     

        public async Task<OperationDetails> CreateAsync(UserDTO userDto, string callbackUrlBase)
        {
            AppUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);            
            if (user == null)
            {
                user = new AppUser { Email = userDto.Email, UserName = userDto.Email }; 
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);
               
                if (result.Errors.Count() > 0)
                {
                    return new OperationDetails(false, result.Errors.ToArray(), "");
                }

                foreach (string role in userDto.Roles)
                {
                    await Database.UserManager.AddToRoleAsync(user.Id, role);
                }
                
                ClientProfile clientProfile = new ClientProfile { ClientProfileID = user.Id, Adress = userDto.Address, Name = userDto.Name };
                Database.Clients.Create(clientProfile);
                var code = await Database.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = callbackUrlBase + "?userId=" + user.Id + "&code=" + HttpUtility.UrlEncode(code);

                await Task.Run(()=> Database.UserManager.SendEmailAsync(user.Id, "Подтверждение электронной почты",
                       "Для завершения регистрации перейдите по ссылке:: <a href=\""
                                                       + callbackUrl + "\">завершить регистрацию</a>"));
                await Database.SaveAsync();
                return new OperationDetails(true, "Пользователь успешно создан", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким именем уже существует", "Email");
            }
        }
        public async Task<ClaimsIdentity> AuthenticateAsync(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            AppUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            if (user != null)
            {                
                claim = await Database.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                if (claim.IsAuthenticated)
                {
                    await Task.Run(() => Database.UserManager.ResetAccessFailedCountAsync(user.Id));                   
                }
            }
            return claim;
        }

        public async Task<OperationDetails> IsEmailConfirmedAsync(UserDTO userDto)
        {
            AppUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            return new OperationDetails(user.EmailConfirmed, user.EmailConfirmed==true ? "Email подтержден" : "Email не подтвержден", "" );
        }

        public async Task<OperationDetails> ConfirmEmailAsync(string userId, string code)
        {

            var result = await Database.UserManager.ConfirmEmailAsync(userId, code);
            return new OperationDetails(result.Succeeded, 
                    result.Succeeded ? "Пользователь активирован" : "Ошибка активации", 
                    "");
        }

        public async Task<OperationDetails> ResetPasswordAsync(string email, string callbackUrlBase)
        {
            var user = await Database.UserManager.FindByEmailAsync(email);
            if (user == null || !(await Database.UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                return new OperationDetails(false, $"Пользователь с email {email} не найден", "");                
            }
            string code = await Database.UserManager.GeneratePasswordResetTokenAsync(user.Id);

            var callbackUrl = callbackUrlBase + "?code=" + HttpUtility.UrlEncode(code);
            await Task.Run(() => Database.UserManager.SendEmailAsync(user.Id, "Сброс пароля",
                "Для сброса пароля, перейдите по ссылке <a href=\"" + callbackUrl + "\">сбросить</a>"));
            return new OperationDetails(true, "Сброс пароля прошёл успешно, письмо отправлено", "");
        }

        public async Task<OperationDetails> DoResetPaswordAsync(ResetPasswordViewModel model)
        {
            var user = await Database.UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return new OperationDetails(false, "Пользователь не найден", "");
            }

            var result = await Database.UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return new OperationDetails(true, "Пароль сброшен", "");
            }
            return new OperationDetails(false, "Ошибка при сбросе пароля", "");
        }

        public async Task<int> CheckForAttemptsAsync(string email)
        {
            AppUser user = await Database.UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return 4;
            }
            
            await Database.UserManager.AccessFailedAsync(user.Id);
            
            int result = await Database.UserManager.GetAccessFailedCountAsync(user.Id);
            return result;
        }

        public async Task<IdentityResult> AccessCounter(string userId)
        {
            var result = await Database.UserManager.AccessFailedAsync(userId);
            var result2 = await Database.UserManager.ResetAccessFailedCountAsync(userId);
            var result3 = await Database.UserManager.GetAccessFailedCountAsync(userId);
            return result;
        }

        public IQueryable<UserDTO> GetUsers()
        {
            var list = Database.UserManager.Users.AsNoTracking().ToList().UserDTOList();            
            return list.AsQueryable<UserDTO>();          
        }

        public async Task<OperationDetails> CreateExceptionAsync(ExceptionDetailDTO exceptionDetail)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<ExceptionDetailDTO, ExceptionDetail>());
            Database.ExceptionDetails.Create(Mapper.Map<ExceptionDetailDTO, ExceptionDetail>(exceptionDetail));
            await Database.SaveAsync();
            return new OperationDetails(true, "Исключение добавлено", "");
        }

        public async Task<UserDTO> GetUserByNameAsync(string userName)
        {
            AppUser user = await Database.UserManager.FindByNameAsync(userName);
            if (user == null)
                return null;
            
            return new UserDTO
            {
                Id = user.Id,
                Address = user.ClientProfile.Adress,
                Email = user.Email,
                Name = user.ClientProfile.Name,
                UserName = user.Email
            };         
        }

        public async Task<OperationDetails> UpdateUserInfoAsync(UserDTO userDTO, string name)
        {
            OperationDetails details = new OperationDetails();
            AppUser user = await Database.UserManager.FindByNameAsync(name);
            user.Email = userDTO.Email;
            user.UserName = userDTO.Email;
            user.ClientProfile.Name = userDTO.Name;
            user.ClientProfile.Adress = userDTO.Address;
            var validEmail = await Database.UserManager.UserValidator.ValidateAsync(user);
            if (!validEmail.Succeeded)
            {
                details =  new OperationDetails(false, validEmail.Errors.ToArray(), "");
                //AddErr
            }

            IdentityResult validPass = null;
            if (userDTO.Password != string.Empty)
            {
                validPass
                    = await Database.UserManager.PasswordValidator.ValidateAsync(userDTO.Password);

                if (validPass.Succeeded)
                {
                    user.PasswordHash =
                        Database.UserManager.PasswordHasher.HashPassword(userDTO.Password);
                }
                else
                {
                    details.Succedeed = false;
                    
                    //details.Messages.Append(validPass.Errors)
                    //AddErrorsFromResult(validPass);
                }
            }

            if ((validEmail.Succeeded && validPass == null) ||
                    (validEmail.Succeeded && userDTO.Password != string.Empty && validPass.Succeeded))
            {                
                IdentityResult result = await Database.UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return new OperationDetails(true, "Данные пользователя изменены", "");
                }
                else
                {
                    details.Messages.Append(result.Errors.FirstOrDefault());
                  //  AddErrorsFromResult(result);
                }
            }

            return details;
        }

        /*
         *  AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail
                    = await UserManager.UserValidator.ValidateAsync(user);

                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }

                IdentityResult validPass = null;
                if (password != string.Empty)
                {
                    validPass
                        = await UserManager.PasswordValidator.ValidateAsync(password);

                    if (validPass.Succeeded)
                    {
                        user.PasswordHash =
                            UserManager.PasswordHasher.HashPassword(password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }

                if ((validEmail.Succeeded && validPass == null) ||
                        (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }
            return View(user);
        }
         */

        //начальная инициализация БД
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new AppRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await CreateAsync(adminDto, null);
        }
        public void Dispose()
        {
            Database.Dispose();
        }

      
    }
}
