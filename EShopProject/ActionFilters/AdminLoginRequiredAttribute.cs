using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShopProject.ActionFilters
{
    public class AdminLoginRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            object memberObject = filterContext.HttpContext.Session["AdminMember"];

            if (memberObject == null || !(memberObject is User))
            {
                filterContext.Result = new RedirectResult("/Admin/Authentication", true);
            }
        }
    }
}