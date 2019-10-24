using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserStore.BLL.DTO;
using UserStore.BLL.Interfaces;

namespace UserStore.WEB.Filters
{
    public class ExceptionLoggerAttribute : IExceptionFilter
    {
        private IExceptionService service;
        
        public ExceptionLoggerAttribute(IExceptionService service)
        {
            this.service = service;
        }

        public void OnException(ExceptionContext filterContext)
        {
            ExceptionDetailDTO exceptionDetail = new ExceptionDetailDTO()
            {
                ExceptionMessage = filterContext.Exception.Message,
                StackTrace = filterContext.Exception.StackTrace,
                ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                ActionName = filterContext.RouteData.Values["action"].ToString(),
                Date = DateTime.Now
            };
            service.CreateExceptionAsync(exceptionDetail);
            filterContext.Result = new ViewResult { ViewName= "Error" }; // RedirectResult("Shared/Error");
            filterContext.ExceptionHandled = true;
        }
    }
}