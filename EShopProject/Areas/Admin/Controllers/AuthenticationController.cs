using EShopProject.DataAccess.Entity;
using EShopProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShopProject.Areas.Admin.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Admin/Authentication
        public ActionResult Index()
        {
            return RedirectToAction("SignIn", new { area = "Admin", controller = "Authentication" });
        }

        public ActionResult SignIn()
        {
            SignInModel model = new SignInModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult SignIn(SignInModel model)
        {
            using (DBContext db = new DBContext())
            {
                User user = db.User.Include("Customer").AsNoTracking().Where(s => s.EmailAddress == model.EmailAddress && !s.Deleted && s.MyAuthorization == MyAuthorization.Admin).FirstOrDefault();

                if (user != null)
                {
                    if (user.Password == model.Password)
                    {
                        Session["AdminMember"] = user;
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            TempData.Remove("ResultMessage");
            TempData["ResultFaileMessage"] = "Kullanıcı Adınız yada şifreniz hatalı. Lütfen tekrar deneyiniz.";
            return View("Index", model);
        }

        //public ActionResult SignOut(UserInformation model)
        //{
        //    if ((MyEntity.User)Session["Member"] != null)
        //    {
        //        Session["Member"] = null;
        //        TempData["FailureMessage"] = model.FullName + " yine görüşücez ";

        //        return View("SignIn");
        //    }

        //    return RedirectToAction("Index", "Home");
        //}

        //public ActionResult GetUserInformation()
        //{
        //    if (Session["Member"] != null)
        //    {
        //        User sessionUser = (MyEntity.User)Session["Member"];
        //        UserInformation user = new UserInformation(sessionUser.Id, sessionUser.FirstName, sessionUser.LastName, sessionUser.UserName);

        //        return PartialView("_TopRightMenu_User", user);
        //    }

        //    return RedirectToAction("Index", "Home");
        //}
    }
}