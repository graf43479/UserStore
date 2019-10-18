using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using UserStore.BLL.Interfaces;
using UserStore.BLL.Models;
using UserStore.WEB.Models;

namespace UserStore.WEB.Controllers
{
    public class RoleAdminController : Controller
    {
        private IRoleService service;

        public RoleAdminController(IRoleService roleService)
        {
            service = roleService;
        }


        public ActionResult Index(string roleId, string userId)
        {

            UsersPerRoleViewModel model = new UsersPerRoleViewModel();
            model.UsersPerRole = service.GetUsersPerRole(roleId, userId);          
            return View(model);
        }

       
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                //IdentityResult result
                //    = await service.Create(name);

                OperationDetails result = await service.Create(name);
                
                if (result.Succedeed)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    //AddErrorsFromResult(result.Message);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            //TODO: заменить имена всех таксов на ASYNC
            OperationDetails result = await service.Delete(id);

            if (result.Succedeed)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error", result.Message);
            }
        }

        
        public async Task<ActionResult> Edit(string id)
        {
            RoleEditModel model = await service.GetRoleEditModel(id);

            return View(model);
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {            
            if (ModelState.IsValid)
            {
                OperationDetails result = await service.AddToRoleAsync(model.RoleName, model.IdsToAdd);
                if (!result.Succedeed)
                {
                    //TODO:: сделать накопление ошибок в лист
                    return View("Error", new string[] { result.Message });
                }

                result = await service.RemoveFromRoleAsync(model.RoleName, model.IdsToDelete);

                if (result.Succedeed)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", new string[] { result.Message });
                }                             
            }
            return View("Error", new string[] { "Роль не найдена" });
        }
        

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        private void Roles()
        {
            //HttpContext.GetOwinContext().GetUserManager<AppRoleManager> Authentication;
        }
    }
}