using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UserStore.BLL.DTO;
using UserStore.BLL.Infrastructure;
using UserStore.BLL.Interfaces;
using UserStore.BLL.Services;
using UserStore.WEB.Models;

namespace UserStore.WEB.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // private IServiceCreator service;
        private IUserService service;

        public AccountController(IUserService userService)
        {
            service = userService;
        }   

        //private IUserService UserService
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().GetUserManager<IUserService>();
        //    }
        //}
       

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [AllowAnonymous]
       // public ActionResult Login()
        public ActionResult Login(string returnUrl)
        {            
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "В доступе отказано" });
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await service.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    bool isConfirmed = await service.IsEmailConfirmedAsync(userDto);
                    if(isConfirmed)
                    {
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);

                        if (String.IsNullOrEmpty(returnUrl))
                        {
                            RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            Redirect(returnUrl);
                        }
                    }
                    else
                    {
                       ModelState.AddModelError("", "Пользователь не активизирован");
                    }
                }
            }
            return View(model);
        }
                
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Address = model.Address,
                    Name = model.Name,
                    //Role = "user"
                    Roles = new string[] { "user" }
                };
                OperationDetails operationDetails = await service.Create(userDto);                
                if (operationDetails.Succedeed)
                    return View("SuccessRegister");
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }


        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            //var result = await service.ConfirmEmailAsync(userId, code);
            OperationDetails result = new OperationDetails(true, "","");
            return View(result.Succedeed ? "ConfirmEmail" : "Error");
        }


        private async Task SetInitialDataAsync()
        {
            await service.SetInitialData(new UserDTO
            {
                Email = "somemail@mail.ru",
                UserName = "somemail@mail.ru",
                Password = "graf43479",
                Name = "Семен Семенович Горбунков",
                Address = "ул. Спортивная, д.30, кв.75",
               // Role = "admin"
                Roles =  new string[] { "admin" }
            }, new List<string> { "user", "admin" });
        }
    }
}