using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
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
                OperationDetails result = await service.CreateAsync(name);                
                if (result.Succedeed)
                {
                    return RedirectToAction("Index");
                }
                else
                {                    
                    AddErrorsFromResult(result);
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {            
            OperationDetails result = await service.DeleteAsync(id);

            if (result.Succedeed)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Error", result.Messages);
            }
        }
                
        public async Task<ActionResult> Edit(string id)
        {
            RoleEditModel model = await service.GetRoleEditModelAsync(id);

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
                    return View("Error",  result.Messages);
                }

                result = await service.RemoveFromRoleAsync(model.RoleName, model.IdsToDelete);

                if (result.Succedeed)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Messages);
                }                             
            }
            return View("Error", new string[] { "Роль не найдена" });
        }
        
        private void AddErrorsFromResult(OperationDetails result)
        {
            foreach (string error in result.Messages)
            {
                ModelState.AddModelError("", error);
            }
        }


    }
}