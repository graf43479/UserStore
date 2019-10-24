using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserStore.WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Dictionary<string, object> data = GetData("Index");
            data.Add("Ключ", "Значение");
            data.Reverse();
           
            return View(data);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            throw new Exception("Some shit was going on!");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private Dictionary<string, object> GetData(string actionName)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("Action", actionName);
            dict.Add("Пользователь", HttpContext.User.Identity.Name);
            dict.Add("Аутентифицирован?", HttpContext.User.Identity.IsAuthenticated);
            dict.Add("Тип аутентификации", HttpContext.User.Identity.AuthenticationType);
           dict.Add("В роли Users?", HttpContext.User.IsInRole("user"));

            return dict;
        }
    }
}