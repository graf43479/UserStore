using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using UserStore.BLL.Interfaces;
using UserStore.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using UserStore.DAL.Entities;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using UserStore.BLL.Models;

namespace UserStore.BLL.Services
{
    public class RoleService : IRoleService
    {
        IUnitOfWork Database { get; set; }

        public RoleService(IUnitOfWork uow)
        {
            Database = uow;
        }
        
        public async Task<OperationDetails> CreateAsync(string roleName)
        {
            var result = await Database.RoleManager.CreateAsync(new AppRole(roleName));
            if (result.Errors.Count() > 0)
            {
                return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
            }
            return new OperationDetails(true, "Роль успешно создана", "");
        }

        public async Task<OperationDetails> DeleteAsync(string roleId)
        {
            AppRole role = await Database.RoleManager.FindByIdAsync(roleId);
            string roleName = role.Name;
            if (role != null)
            {
                IdentityResult result = await Database.RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return new OperationDetails(true, $"Роль {roleName} удалена", "");
                }
                else
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }
            }
            else
            {
                return new OperationDetails(false, "Роль не найдена", "" );
            }
        }

        //TODO: переименование имени роли
        public async Task<OperationDetails> UpdateAsync(RoleDTO roleDto)
        {
            throw new Exception();
        }

        public async Task<RoleEditModel> GetRoleEditModelAsync(string roleId)
        {
            AppRole role = await Database.RoleManager.FindByIdAsync(roleId);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();

            IEnumerable<UserInfoDTO> members
                = Database.UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id)).Select(x=> new UserInfoDTO()
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    Address = x.ClientProfile.Adress,
                    Email = x.Email,
                    Name = x.UserName
                });

            IEnumerable<UserInfoDTO> nonMembers = Database.UserManager.Users.Select(x=>new UserInfoDTO()
            {
                UserId = x.Id,
                UserName = x.UserName,
                Address = x.ClientProfile.Adress,
                Email = x.Email,
                Name = x.UserName
            }).Except(members);

            return new RoleEditModel
            {
                roleId = role.Id,
                Name = role.Name,
                Members = members,
                NonMembers = nonMembers
            };
        }

        private UserInfoDTO GetUserDTO(string userId)
        {
            return Database.UserManager.Users.Select(x => new UserInfoDTO()
            {
                UserId = x.Id,
                UserName = x.UserName,
                Address = x.ClientProfile.Adress,
                Email = x.Email,
                Name = x.UserName
            }).FirstOrDefault(m => m.UserId == userId);             
        }

        private IEnumerable<UserInfoDTO> GetUsersPerRoleDTO(string roleId)
        {
            List<UserInfoDTO> usersInfo = new List<UserInfoDTO>();
            var p = Database.RoleManager.Roles.AsNoTracking().Where(x => x.Id == roleId).FirstOrDefault();
            if (p != null)
            {
                IEnumerable<IdentityUserRole> users = p.Users;
                foreach (IdentityUserRole iu in users)
                {
                    usersInfo.Add(GetUserDTO(iu.UserId));
                }
            }
            return usersInfo;
        }

        public IEnumerable<UsersPerRoleDTO> GetUsersPerRole(string roleId, string userId)
        {
            List<UsersPerRoleDTO> result = new List<UsersPerRoleDTO>();
            var appRoles = (string.IsNullOrEmpty(roleId)) ?
                        Database.RoleManager.Roles.AsNoTracking().ToList() :
                        Database.RoleManager.Roles.AsNoTracking().Where(x => x.Id == roleId).ToList();

            foreach (var role in appRoles)
            {
                UsersPerRoleDTO model = new UsersPerRoleDTO()
                {
                    RoleId = role.Id,
                    RoleName = role.Name,                    
                    Users = GetUsersForRole(role.Id, userId, appRoles)
                };
                result.Add(model);
            }            
            return result;
        }

        private IEnumerable<UserInfoDTO> GetUsersForRole(string roleId, string userId, IEnumerable<AppRole> appRoles)
        {
            var tmp = string.IsNullOrEmpty(userId) ?
                        appRoles.FirstOrDefault(x => x.Id == roleId).Users :
                        appRoles.FirstOrDefault(x => x.Id == roleId).Users.Where(x=>x.UserId==userId);


            IEnumerable<UserInfoDTO> users = from ur in tmp
                                             join u in Database.UserManager.Users on ur.UserId equals u.Id
                                             //where u.EmailConfirmed==true
                                             select new UserInfoDTO()
                                             {
                                                 UserId = u.Id,
                                                 Email = u.Email,
                                                 Address = u.Email,
                                                 Name = u.UserName,
                                                 UserName = u.UserName
                                             };
            return users;
        }

        public async Task<OperationDetails> AddToRoleAsync(string roleName, string[] idsToAdd)
        {            
            foreach (string userId in idsToAdd ?? new string[] { })
            {
                IdentityResult result = await Database.UserManager.AddToRoleAsync(userId, roleName);
                if(!result.Succeeded)                
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }                
            }            
            return new OperationDetails(true, $"Пользователи добавлены в роль {roleName}", "");            
        }

        public async Task<OperationDetails> RemoveFromRoleAsync(string roleName, string[] idsToDelete)
        {
            foreach (string userId in idsToDelete ?? new string[] { })
            {
                IdentityResult result = await Database.UserManager.RemoveFromRoleAsync(userId, roleName);
                if (!result.Succeeded)
                {
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                }
            }
            return new OperationDetails(true, $"Пользователи удалены из роли {roleName}", "");
        }

    }

    
}
