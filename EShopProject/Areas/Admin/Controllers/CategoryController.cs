using EShopProject.DataAccess.Entity;
using EShopProject.MyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShopProject.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            ViewBag.Title = "Kategorler Listesi";
            List<Category> categoryList;

            using (DBContext db = new DBContext())
                categoryList = db.Category.AsNoTracking().Include("ProductList").Include("ParentCategory").ToList();

            if (categoryList == null)
                categoryList = new List<Category>();

            return View(categoryList);
        }

        public ActionResult Create(Category category)
        {
            ViewBag.Title = "Kategori Oluşturma";

            FillDropDownList();

            if (category == null)
                category = new Category();

            return View(category);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Title = "Kategori Düzenleme";

            FillDropDownList();

            Category category;

            using (DBContext db = new DBContext())
            {
                category = db.Category.Include("ParentCategory").AsNoTracking().Where(c => c.Id == id).FirstOrDefault();

                if (category == null)
                    return HttpNotFound();
            }

            return View("Create", category);
        }

        [HttpPost]
        public ActionResult Save(Category model)
        {
            if (!CheckModelValue(model))
            {
                return RedirectToAction("Create", model);
            }

            using (DBContext db = new DBContext())
            {
                try
                {
                    if (model.Id == 0)
                    {
                        db.Entry(model).State = System.Data.Entity.EntityState.Added;
                        TempData["ResultMessage"] = model.Name + " Kategori Başarıyla Eklenmiştir.";
                    }
                    else
                    {
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        TempData["ResultMessage"] = model.Name + " Kategori Başarıyla Güncellenmiştir.";
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    TempData.Remove("ResultMessage");
                    TempData["ResultFaileMessage"] = "İşlemi başarısız tekrar deneyin. \n " + ex.Message;
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        private void FillDropDownList()
        {
            using (DBContext db = new DBContext())
            {
                List<Category> allCategory = db.Category.AsNoTracking().Where(c => c.Deleted == false).ToList();

                ViewBag.CategorySelectList = new SelectList(allCategory, "Id", "Name");
            }
        }

        [HttpPost]
        public ActionResult DeleteSubmitted(int id)
        {
            using (DBContext db = new DBContext())
            {
                Category category = db.Category.AsNoTracking().Where(c => c.Id == id).FirstOrDefault();

                if (category == null)
                    return HttpNotFound();

                try
                {
                    category.Deleted = true;
                    db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TempData["ResultMessage"] = category.Name + " Kategori Başarıyla Silinmiştir.";

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["ResultFaileMessage"] = "Silme işlemi başarısız tekrar deneyin.";
                    return RedirectToAction("Index");
                }

            }
        }

        private bool CheckModelValue(Category model)
        {
            bool valueCheck = false;

            if (ValueCheckHelper.EmptyCheckString(model.Name, model.Description))
            {
                TempData["ResultFaileMessage"] = "Gerekli olan değerleri doldurmalısınız.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.Name, 3, 30))
            {
                TempData["ResultFaileMessage"] = "Ürün adını kontrol ediniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.Name, 3, 1000))
            {
                TempData["ResultFaileMessage"] = "Ürün açıklamasını kontrol ediniz.";
                return valueCheck;
            }

            return true;
        }
    }
}