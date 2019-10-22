using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UserStore.BLL.Interfaces;

namespace UserStore.WEB.Controllers
{
    [Authorize(Roles="admin")]
    public class AdminController : Controller
    {

        private IUserService service;

        public AdminController(IUserService userService)
        {
            service = userService;
        }      

        public ActionResult Index()
        {
            var p = service.GetUsers().ToList();
            return View(p);
        }
    }
}