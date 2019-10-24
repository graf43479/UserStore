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
using UserStore.BLL.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using UserStore.WEB.Filters;

namespace UserStore.WEB.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserService service;        

        public AccountController(IUserService userService)
        {
            service = userService;
        }   
             
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
                
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {            
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home"); //  View("Error", new string[] { "В доступе отказано" });
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            //Два способа подсчета неудачных попыток входа
            //1. Через встроенные средства Identity. Минус: работает только для зарегистрированного логина
            //2. Через сессию. Минус: сессия может сбрасываться роботом. 
            //Тем не менее совместим оба подхода. Можно будет ещё куки добавить       
            
            Session["attempt"] = (Session["attempt"]==null) ? 0 : (int)Session["attempt"] + 1;                        
            if ((Session["Captcha"] == null || Session["Captcha"].ToString() != model.Captcha) && (int)Session["attempt"] > 3)
            {
                ModelState.AddModelError("Captcha", "Сумма введена неверно! Пожалуйста, повторите ещё раз!");
                return View(model);
            }
                
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await service.AuthenticateAsync(userDto);
                if (claim == null)
                {                    
                    Session["attempt"] = await service.CheckForAttemptsAsync(model.Email);                    
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                    
                }
                else
                {
                    OperationDetails isConfirmed = await service.IsEmailConfirmedAsync(userDto);
                    if(isConfirmed.Succedeed)
                    {
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);

                        Session["attempt"] = 0;
                        if (String.IsNullOrEmpty(returnUrl) || returnUrl.Contains(Url.Action("Login","Account")))
                        {
                           return RedirectToAction("Index", "Home", null);
                        }
                        else
                        {
                            Redirect(returnUrl);
                        }
                    }
                    else
                    {
                        AddErrorsFromResult(isConfirmed);                       
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
                string callbackUrlBase = Url.Action("ConfirmEmail", "Account", null, protocol: Request.Url.Scheme);
                OperationDetails operationDetails = await service.CreateAsync(userDto, callbackUrlBase);
                if (operationDetails.Succedeed)
                    return View("SuccessRegister");
                else
                    AddErrorsFromResult(operationDetails);                    
            }
            return View(model);
        }

        //Подтердждение активации аккаунта/изменение забытого пароля
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error", new string[] { "Строка подтверждения некорректна!"});
            }
            OperationDetails result = await service.ConfirmEmailAsync(userId, code);
            //TODO: сделать страницы обработки результата
            //return View(result.Succedeed ? "ConfirmEmail" : "Error");
            return View(result.Succedeed ? "SuccessRegister" : "Error");
        }

        //страница указания email для восстановления пароля
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string callbackUrlBase = Url.Action("ResetPassword", "Account", null, protocol: Request.Url.Scheme);
                OperationDetails result = await service.ResetPasswordAsync(model.Email, callbackUrlBase);
                if (result.Succedeed)
                {
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                else
                {
                    return View("ForgotPasswordConfirmation");
                }
            }
            return View(model);
        }


        //Страница сообщения о том, что на почту выслана ссылка для восстановления пароля
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        //Подтверждение установки нового пароля
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            OperationDetails result = await service.DoResetPaswordAsync(model);

            if (!result.Succedeed)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            
            return RedirectToAction("ResetPasswordConfirmation", "Account");
                        
            //TODO: метод AddErrors
            //AddErrors(result);         
        }

        //TODO: смена профильных данных
        public ActionResult EditAccount()
        {
          //  string userName = HttpContext.User.Identity.Name;
          //взять имя из контекста, вернуть в модель,
          //приянть обновленную модель и переслать на update
            RegisterModel model = new RegisterModel();
            
            return View(model);
        }

        //public async Task<ActionResult> Edit(string id)
        //{
        //    AppUser user = await UserManager.FindByIdAsync(id);
        //    if (user != null)
        //    {
        //        return View(user);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}
        /*
         *    public async Task<ActionResult> Edit(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, string email, string password)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail
                    = await UserManager.UserValidator.ValidateAsync(user);

                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }

                IdentityResult validPass = null;
                if (password != string.Empty)
                {
                    validPass
                        = await UserManager.PasswordValidator.ValidateAsync(password);

                    if (validPass.Succeeded)
                    {
                        user.PasswordHash =
                            UserManager.PasswordHasher.HashPassword(password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }

                if ((validEmail.Succeeded && validPass == null) ||
                        (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }
            return View(user);
        }
         */

        [AllowAnonymous]
        public ActionResult CaptchaImage(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            //generate new question
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer
            Session["Captcha" + prefix] = a + b;

            //image stream
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                            (rand.Next(0, 255)),
                            (rand.Next(0, 255)),
                            (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                //add question
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }

        private void AddErrorsFromResult(OperationDetails result)
        {
            foreach (string error in result.Messages)
            {
                ModelState.AddModelError("", error);
            }
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