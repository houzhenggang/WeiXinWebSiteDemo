using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginOn(LoginModel model)
        {

            if (model.UserName == "admin" && model.PassWord == "admin")
            {
                return Redirect("/Admin/Admin/Index");
            }
            else
            {
                ModelState.AddModelError("", "提供的用户名或密码不正确。");
            }
            
            return View("Index");
        }
    }

    public class LoginModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}