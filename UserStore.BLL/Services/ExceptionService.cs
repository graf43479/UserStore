using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using UserStore.BLL.Interfaces;
using UserStore.DAL.Entities;
using UserStore.DAL.Interfaces;

namespace UserStore.BLL.Services
{
    public class ExceptionService : IExceptionService
    {
        IUnitOfWork Database { get; set; }

        public ExceptionService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> CreateExceptionAsync(ExceptionDetailDTO exceptionDetail)
        {             
            Mapper.Initialize(cfg => cfg.CreateMap<ExceptionDetailDTO, ExceptionDetail>());
            Database.ExceptionDetails.Create(Mapper.Map<ExceptionDetailDTO,ExceptionDetail>(exceptionDetail));
            await Database.SaveAsync();
            return new OperationDetails(true, "Исключение добавлено", "");
        }

        
    }
}
