using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;

namespace UserStore.BLL.Interfaces
{
    public interface IExceptionService
    {
        Task<OperationDetails> CreateExceptionAsync(ExceptionDetailDTO exceptionDetail);
    }
}
