using Microsoft.AspNet.Identity;
using Ninject;
using System;
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
using UserStore.DAL.Repositories;
using System.Web.Mvc;
using UserStore.DAL.Identity;
using Microsoft.AspNet.Identity.Owin;

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
            
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }


        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            //Database.UserManager.PasswordValidator
            AppUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);            
            if (user == null)
            {
                user = new AppUser { Email = userDto.Email, UserName = userDto.Email }; 
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);
               // var result = Database.UserManager.Create(user, userDto.Password);

                if (result.Errors.Count() > 0)
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }

                //await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                foreach (string role in userDto.Roles)
                {
                    await Database.UserManager.AddToRoleAsync(user.Id, role);
                }
                
                ClientProfile clientProfile = new ClientProfile { ClientProfileID = user.Id, Adress = userDto.Address, Name = userDto.Name };
                Database.ClientManager.Create(clientProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Пользователь успешно создан", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким именем уже существует", "Email");
            }

        }
        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            AppUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            if (user != null)
            {
                claim = await Database.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }
            return claim;
        }

        public IQueryable<UserDTO> GetUsers()
        {
            var list = Database.UserManager.Users.AsNoTracking().ToList().UserDTOList();
            

            /*
               public string ClientProfileID { get; set; }
                public string Name { get; set; }
                public string Adress { get; set; }
                public virtual AppUser AppUser { get; set; }
             */

            /*UserDTO
             *   public string Id { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Role { get; set; }
             */

            return list.AsQueryable<UserDTO>();
          //  return  Database.UserManager.Users;
        }


        //private AppRoleManager RoleManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
        //    }
        //}


        //Ienummerable<AppRole> 

        //AppRole
        //Id
        //Name
        //Users

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
            await Create(adminDto);
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
