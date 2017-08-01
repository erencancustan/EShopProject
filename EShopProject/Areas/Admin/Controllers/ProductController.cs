using EShopProject.Areas.Admin.Models;
using EShopProject.DataAccess.Entity;
using EShopProject.MyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShopProject.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin/Product
        public ActionResult Index()
        {
            ViewBag.Title = "Ürünlerin Listesi";

            List<Product> productList;
            List<ProductModel> productModelList = new List<ProductModel>();

            using (DBContext db = new DBContext())
                productList = db.Product.AsNoTracking().Include("Category").Include("ProductToAttributeList").Include("ImageList").ToList();

            foreach (Product entity in productList)
                productModelList.Add(ProductForEntityToModel(entity));

            if (productModelList == null)
                productModelList = new List<ProductModel>();

            return View(productModelList);
        }

        public ActionResult Create(ProductModel model)
        {
            ViewBag.Title = "Ürün Ekleme";

            FillDropDownList();

            if (model == null)
                model = new ProductModel();

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Title = "Ürünü Düzenleme";

            FillDropDownList();

            Product product;

            using (DBContext db = new DBContext())
            {
                product = db.Product.Include("Category").Include("ProductToAttributeList").Include("ImageList").AsNoTracking().Where(c => c.Id == id).FirstOrDefault();

                if (product == null)
                    return HttpNotFound();
            }

            return View("Create", ProductForEntityToModel(product));
        }
        [HttpPost]
        public ActionResult Save(ProductModel model, IEnumerable<HttpPostedFileBase> files)
        {
            //foreach (var file in files)
            //{
            //    if (file.ContentLength > 0)
            //    {
            //        var fileName = Path.GetFileName(file.FileName);
            //        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            //        file.SaveAs(path);
            //    }
            //}

                GetAllAttributeAndFillModel(HttpContext.Request.Form["productPropertyType"], HttpContext.Request.Form["productPropertyValue"], model);

            if (CheckModelValue(model) ||(CheckAttribute(model.ProductToAttributeList)))
            {
                return RedirectToAction("Create", model);
            }

            using (DBContext db = new DBContext())
            {
                try
                {
                    Product product = ProductModelForModelToEntity(model);

                    if (product.Id == 0)
                    {
                        db.Entry(product).State = System.Data.Entity.EntityState.Added;
                        TempData["ResultMessage"] = product.Name + " Ürün Başarıyla Eklenmiştir.";
                    }
                    else
                    {
                        db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                        db.Entry(product.ProductToAttributeList).State = System.Data.Entity.EntityState.Modified;
                        db.Entry(product.ImageList).State = System.Data.Entity.EntityState.Modified;
                        TempData["ResultMessage"] = product.Name + " Ürün Başarıyla Güncellenmiştir.";
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

        private void GetAllAttributeAndFillModel(string productPropertyType, string productPropertyValue, ProductModel model)
        {
            try
            {
                ProductToAttribute pTANode;
            
                string[] productPropertyTypeList = productPropertyType.Split(',');
                string[] productPropertyValueList = productPropertyValue.Split(',');

                for (int i = 0; i < productPropertyTypeList.Count(); i++)
                {
                    pTANode = new ProductToAttribute();
                    pTANode.ProductId = model.Id;
                    pTANode.AttributeId = Convert.ToInt32(productPropertyTypeList[i]);
                    pTANode.Value = productPropertyValueList[i].ToString();

                    model.ProductToAttributeList.Add(pTANode);
                }
            }
            catch (Exception)
            {
                TempData["ResultFaileMessage"] = "Ürün Özelliklerinde bir hata oluştu.";
                RedirectToAction("Create", model);
            }
        }

        private void FillDropDownList()
        {
            using (DBContext db = new DBContext())
            {
                List<Category> allCategory = db.Category.AsNoTracking().Where(c => c.Deleted == false).ToList();
                ViewBag.CategorySelectList = new SelectList(allCategory, "Id", "Name");

                List<DataAccess.Entity.Attribute> allAttribute = db.Attribute.AsNoTracking().Where(c => c.Deleted == false).ToList();
                ViewBag.AttributeSelectList = new SelectList(allAttribute, "Id", "Name");
            }
        }

        [HttpPost]
        public ActionResult GetAllAttribute()
        {
            using (DBContext db = new DBContext())
            {
                List<DataAccess.Entity.Attribute> allAttribute = db.Attribute.AsNoTracking().Where(c => c.Deleted == false).ToList();
                SelectList AttributeSelectList = new SelectList(allAttribute, "Id", "Name", "AttributeValueType");

                return Json(AttributeSelectList, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteSubmitted(int id)
        {
            using (DBContext db = new DBContext())
            {
                Product product = db.Product.AsNoTracking().Where(c => c.Id == id).FirstOrDefault();

                if (product == null)
                    return HttpNotFound();

                try
                {
                    product.Deleted = true;
                    db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    TempData["ResultMessage"] = product.Name + " Ürün Başarıyla Silinmiştir.";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["ResultFaileMessage"] = "Silme işlemi başarısız tekrar deneyin.\n" + ex.Message;
                    return RedirectToAction("Index");
                }

            }
        }

        private bool CheckModelValue(ProductModel model)
        {
            bool valueCheck = true;

            if (ValueCheckHelper.EmptyCheckString(model.Name, model.Description))
            {
                TempData["ResultFaileMessage"] = "Gerekli olan değerleri doldurmalısınız.";
                return valueCheck;
            }
            if (model.ProductToAttributeList.Count() < 1)
            {
                TempData["ResultFaileMessage"] = "Ürün Özelliği bellirtmediniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.NullAndNotZeroCheckNumber(model.Category.Id, model.Price, model.UnitInStock))
            {
                TempData["ResultFaileMessage"] = "Hatalı sayısal değer girdiniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.Name, 3, 50))
            {
                TempData["ResultFaileMessage"] = "Ürün Adını kontrol ediniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.Description, 3, 500))
            {
                TempData["ResultFaileMessage"] = "Ürün Açıklamasını kontrol ediniz.";
                return valueCheck;
            }

            return !valueCheck;
        }

        private bool CheckAttribute(List<ProductToAttribute> productToAttributeList)
        {
            bool valueCheck = true;

            for (int i = 0; i < productToAttributeList.Count; i++)
            {
                for (int j = i+1 ; j < productToAttributeList.Count; j++)
                {
                    if (productToAttributeList[i].AttributeId == productToAttributeList[j].AttributeId)
                    {
                        TempData["ResultFaileMessage"] = "Aynı özelliği birden fazla kullanamazsınız.";
                        return valueCheck;
                    }
                }
            }
            return !valueCheck;
        }

        private ProductModel ProductForEntityToModel(Product entity)
        {
            ProductModel productModel = new ProductModel();

            productModel.Id = entity.Id;
            productModel.Name = entity.Name;
            productModel.Description = entity.Description;
            productModel.Price = entity.Price;
            productModel.UnitInStock = entity.UnitInStock;
            productModel.Deleted = entity.Deleted;

            productModel.Category = new CategoryModel();
            productModel.Category.Id = entity.CategoryId;
            productModel.Category.Name = entity.Category.Name;
            productModel.Category.Description = entity.Category.Description;
            productModel.Category.ParentId = entity.Category.ParentId;

            foreach (var item in entity.ProductToAttributeList)
            {
                productModel.ProductToAttributeList.Add(item);
            }

            return productModel;
        }

        private Product ProductModelForModelToEntity(ProductModel model)
        {
            Product product = new Product();

            product.Id = model.Id;
            product.CategoryId = model.Category.Id;
            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.UnitInStock = model.UnitInStock;
            product.Deleted = model.Deleted;

            foreach (var item in model.ProductToAttributeList)
            {
                product.ProductToAttributeList.Add(item);
            }

            return product;
        }

        public ActionResult UploadData()
        {
            HttpPostedFileBase file = HttpContext.Request.Files[0];
            
            using (BinaryReader reader = new BinaryReader(file.InputStream))
            {
                byte[] deger = reader.ReadBytes(file.ContentLength);

                if (Session["ImageFile"] == null)
                {
                    Session["ImageFile"] = deger;
                }
                //else
                //{
                //    Session["ImageFile"] = UtilityManager.ByteBirlestir((byte[])Session["ImageFile"], deger);
                //}
                //if (file.ContentLength < 10000)
                //{

                //    _db.Dosyalar.Add(new Dosya
                //    {
                //        Deger = (byte[])Session["ImageFile"],
                //        DosyaAdi = file.FileName,
                //        DosyaBoyutu = ((byte[])Session["ImageFile"]).Length,
                //        DosyaTipi = file.ContentType,
                //        IkonBootstrap = UtilityManager.setIcon(file.ContentType),
                //        IkonResim = UtilityManager.setImage(file.ContentType),
                //        KayitTarihi = DateTime.Now,
                //    });
                //    _db.SaveChanges();

                //    Session.Remove("Alafortanfoni");
                //}
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public ActionResult ImageUpload()
        {
            Image newImage;
            //TODO : sadece Resim olduğu kontrol yapılmalı boyut kontrolu eklenmeli
            var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };

            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    newImage = new Image();
                    HttpPostedFileBase file = Request.Files[fileName];
                    fName = file.FileName;//TODO:resimin adını productID 1-2- gibi olabilir
                    if (file != null && file.ContentLength > 0)
                    {
                        var path = Path.Combine(Server.MapPath("~/assets/images/product-image"));
                        string pathString = System.IO.Path.Combine(path.ToString());
                        var fileName1 = Path.GetFileName(file.FileName);
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                        var uploadpath = string.Format("{0}\\{1}", pathString, file.FileName);

                        newImage.ProductId = 123;//TODO:ürün Id gelicek
                        newImage.FilePath = uploadpath;
                        newImage.Ranking = 2;//TODO: resim sirasi ile 
                        newImage.Deleted = false;

                        file.SaveAs(uploadpath);
                    }
                }
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
                return Json(new { Message = fName });
            else
                return Json(new { Message = "Resim Kayıt İslemi Sırasında hata oluştu." });
        }

        //[HttpPost]
        //public ActionResult Index(FormCollection fc, HttpPostedFileBase file)
        //{
        //    //tbl_details tbl = new tbl_details();
        //    //var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
        //    //tbl.Id = fc["Id"].ToString();
        //    //tbl.Image_url = file.ToString(); //getting complete url  
        //    //tbl.Name = fc["Name"].ToString();
        //    //var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
        //    //var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
        //    //if (allowedExtensions.Contains(ext)) //check what type of extension  
        //    //{
        //    //    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
        //    //    string myfile = name + "_" + tbl.Id + ext; //appending the name with id  
        //    //    // store the file inside ~/project folder(Img)  
        //    //    var path = Path.Combine(Server.MapPath("~/Img"), myfile);
        //    //    tbl.Image_url = path;
        //    //    obj.tbl_details.Add(tbl);
        //    //    obj.SaveChanges();
        //    //    file.SaveAs(path);
        //    //}
        //    //else
        //    //{
        //    //    ViewBag.message = "Please choose only Image file";
        //    //}
        //    return View();
        //}
        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Test(ImageModel model)
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {

                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);

                    }

                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }


    }
}