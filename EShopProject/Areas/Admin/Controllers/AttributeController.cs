using System;
using System.Collections.Generic;
using System.Linq;
using EShopProject.DataAccess;
using System.Web;
using System.Web.Mvc;
using EShopProject.MyLib;
using EShopProject.DataAccess.Entity;

namespace EShopProject.Areas.Admin.Controllers
{
    public class AttributeController : Controller
    {
        // GET: Admin/Attribute
        public ActionResult Index()
        {
            ViewBag.Title = "Özellikler Listesi";
            //List<DataAccess.Entity.Attribute> tagList;

            //using (DBContext db = new DBContext())
            //    tagList = db.Tag.AsNoTracking().Include("articles").ToList();

            //if (tagList == null)
            //    tagList = new List<Tag>();

            return View();
        }

        public ActionResult Create(DataAccess.Entity.Attribute model)
        {
            ViewBag.Title = "Özellik Oluşturma";

            FillDropDownList();

            if (model == null)
                model = new DataAccess.Entity.Attribute();

            return View(model);
        }

        //public ActionResult Edit(int id)
        //{
        //    ViewBag.Title = "Etiket Güncelleme";

        //    Tag tag;

        //    using (DBContext db = new DBContext())
        //    {
        //        tag = db.Tag.Include("articles").AsNoTracking().Where(c => c.Id == id).FirstOrDefault();

        //        if (tag == null)
        //            return HttpNotFound();
        //    }

        //    return View("Create", tag);
        //}

        [HttpPost]
        public ActionResult Save(DataAccess.Entity.Attribute model)
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
                       // model.AttributeGroupId = 2;
                        db.Entry(model).State = System.Data.Entity.EntityState.Added;
                        TempData["ResultMessage"] = model.Name + " Özellik Başarıyla Eklenmiştir.";
                    }
                    else
                    {
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        TempData["ResultMessage"] = model.Name + " Özellik Başarıyla Güncellenmiştir.";
                    }

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    TempData.Remove("ResultMessage");
                    TempData["ResultFaileMessage"] = "İşlemi başarısız tekrar deneyin.\n" + ex.Message;
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        private void FillDropDownList()
        {
            using (DBContext db = new DBContext())
            {
                List<AttributeGroup> allAttributeGroup = db.AttributeGroup.AsNoTracking().Where(c => c.Deleted == false).ToList();
                ViewBag.AttributeGroupSelectList = new SelectList(allAttributeGroup, "Id", "Name");
            }
        }

        [HttpPost]
        public ActionResult DeleteSubmitted(int id)
        {
            using (DBContext db = new DBContext())
            {
                DataAccess.Entity.Attribute tag = db.Attribute.AsNoTracking().Where(c => c.Id == id).FirstOrDefault();

                if (tag == null)
                    return HttpNotFound();

                try
                {
                    tag.Deleted = true;
                    db.Entry(tag).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TempData["ResultMessage"] = tag.Name + " Etiket Başarıyla Silinmiştir.";

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["ResultFaileMessage"] = "Silme işlemi başarısız tekrar deneyin.";
                    return RedirectToAction("Index");
                }

            }
        }

        private bool CheckModelValue(DataAccess.Entity.Attribute model)
        {
            bool valueCheck = false;

            if (ValueCheckHelper.EmptyCheckString(model.Name, model.Description))
            {
                TempData["ResultFaileMessage"] = "Gerekli olan değerleri doldurmalısınız.";
                return valueCheck;
            }
            if (ValueCheckHelper.NullAndNotZeroCheckNumber(model.AttributeValueType))
            {
                TempData["ResultFaileMessage"] = "Hatalı sayısal değer girdiniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.Name, 1, 50))
            {
                TempData["ResultFaileMessage"] = "Özellik Adını kontrol ediniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.Description, 3, 200))
            {
                TempData["ResultFaileMessage"] = "Özellik Açıklamasını kontrol ediniz.";
                return valueCheck;
            }


            return !valueCheck;
        }
    }
}