using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.BLL.Helpers;
using UserStore.BLL.Infrastructure;
using UserStore.BLL.Interfaces;
using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;

namespace UserStore.BLL.Services
{
    public class AdminService : IAdminService
    {
        IUnitOfWork Database { get; set; }

        public AdminService(IUnitOfWork uow)
        {
            Database = uow;           
        }

        public async Task<IQueryable<UserDTO>> GetUsersAsync()
        {
            var list = (await Database.UserManager.Users.AsNoTracking().ToListAsync()).UserDTOList();
            return list.AsQueryable();
        }


        public async Task<OperationDetails> DeleteUserAsync(string id)
        {
            
            AppUser user = await Database.UserManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await Database.UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return new OperationDetails(true, "Пользователь удален", "");
                }
                else
                {
                    return new OperationDetails(false, result.Errors.ToArray(), "");
                }
            }
            else
            {
                return new OperationDetails(false, "Пользователь не найден", "");
            }
        }

        public async Task<IEnumerable<ExceptionDetailDTO>> GetLogAsync()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<ExceptionDetail, ExceptionDetailDTO>());
            var exceptions = await Task.Run(() => Database.ExceptionDetails.GetAll()); 
            
            return Mapper.Map<IEnumerable<ExceptionDetail>, IEnumerable<ExceptionDetailDTO>>(exceptions); 
        }

        public async Task<OperationDetails> DeleteExceptionDetailAsync()
        {
            
            foreach (ExceptionDetail exception in Database.ExceptionDetails.GetAll())
            {
                Database.ExceptionDetails.Delete(exception);
            }
            await Database.SaveAsync();
            return new OperationDetails(true, "Лог ошибок очищен", "");        
        }

        public async Task<OperationDetails> DeleteExceptionDetailByIDAsync(int id)
        {
            ExceptionDetail exception = await Task.Run(()=> Database.ExceptionDetails.Get(id));
            if (exception != null)
            {
                Database.ExceptionDetails.Delete(exception);
                await Database.SaveAsync();
                return new OperationDetails(true, "Объект удален", "");
            }
            else
            {
                return new OperationDetails(false, "Возникла ошибка при удалении объекта " + exception.Id, "");
            }              
        }

    }
}
