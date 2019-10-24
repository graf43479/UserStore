using Ninject;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using UserStore.BLL.Interfaces;
using UserStore.WEB.Filters;
using UserStore.WEB.Infrastructure;

namespace UserStore.WEB
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());          
        }        
    }
}
