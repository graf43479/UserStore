using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.BLL.DTO
{
    public class ExceptionDetailDTO
    {
        public int Id { get; set; }
        public string ExceptionMessage { get; set; }    // сообщение об исключении
        public string ControllerName { get; set; }      // контроллер, где возникло исключение
        public string ActionName { get; set; }          // действие, где возникло исключение
        public string StackTrace { get; set; }          // стек исключения
        public DateTime Date { get; set; }
    }
}
