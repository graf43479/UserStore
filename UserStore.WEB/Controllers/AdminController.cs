using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using UserStore.BLL.Interfaces;

namespace UserStore.WEB.Controllers
{
    [Authorize(Roles="admin")]
    public class AdminController : Controller
    {

        private IAdminService service;

        public AdminController(IAdminService adminService)
        {
            service = adminService;
        }      

        public async Task<ActionResult> Index()
        {
            var p = await service.GetUsersAsync();
            return View(p.ToList());
        }


        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            OperationDetails result = await service.DeleteUserAsync(id);
            if (result.Succedeed)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error", result.Messages);
            }
        }

        public async Task<ActionResult> ExceptionLogger()
        {
            IEnumerable<ExceptionDetailDTO> exceptions = await service.GetLogAsync();         
            return View(exceptions);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteLogger(int? id)
        {
            if (id == null)
            {
                OperationDetails result = await service.DeleteExceptionDetailAsync();
            }
            else
            {
                OperationDetails result = await service.DeleteExceptionDetailByIDAsync((int)id);
            }
             return RedirectToAction("ExceptionLogger");
        }
    }
}