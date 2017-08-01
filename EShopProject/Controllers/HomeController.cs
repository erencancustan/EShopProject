using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShopProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        static List<Category> GetCategoryListFromDatabase()
        {

            List<Category> allCategory;
            using (DBContext db = new DBContext())
            {
                allCategory = db.Category.Include("ParentCategory").Include("SubCategories").Where(c => c.Deleted == false).ToList();
            }

            return allCategory;
        }

        public ActionResult GetMenuCategory()
        {

            List<Category> allCategory;
            using (DBContext db = new DBContext())
            {
                allCategory = db.Category.Include("ParentCategory").Include("SubCategories").Where(c => c.Deleted == false && c.ParentId == null).ToList();
            }

            return PartialView("_MenuCategoryView", allCategory);
        }

        public ActionResult GetLeftCategoryMenu()
        {

            List<Category> allCategory;
            using (DBContext db = new DBContext())
            {
                allCategory = db.Category.Include("ParentCategory").Include("SubCategories").Where(c => c.Deleted == false && c.ParentId == null).ToList();
            }

            return PartialView("_LeftCategoryMenu", allCategory);
        }

        public ActionResult GetProductSlider()
        {

            List<Product> sliderproduct;

            using (DBContext db = new DBContext())
            {
                sliderproduct = db.Product.Include("ImageList").Where(c => c.Deleted == false && c.ImageList.Count > 1).Take(10).ToList();
            }

            return PartialView("_IndexProductSlider", sliderproduct);
        }
    }
}