using EShopProject.DataAccess.Entity;
using EShopProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShopProject.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int id = 0)
        {
            Category category;

            using (DBContext db = new DBContext())
            {
                if (id < 1)
                    id = 1;

                category = db.Category.Include("ParentCategory").Include("SubCategories").Include("ProductList").Include("ProductList.ImageList").Where(c => c.Id == id).FirstOrDefault();

                if (category == null)
                {
                    category = db.Category.Include("ParentCategory").Include("SubCategories").Include("ProductList").Include("ProductList.ImageList").Where(c => c.Id == 1).FirstOrDefault();
                }
            }

            ViewBag.TESTO = GetListFromDatabase();

            return View(category);
        }

        public ActionResult Detail(int id)
        {
            ViewBag.TESTO = GetListFromDatabase();
            Product product;

            using (DBContext db = new DBContext())
            {
                product = db.Product.Include("Category").Include("ProductToAttributeList").Include("ProductToAttributeList.Attribute").Include("ProductToAttributeList.Attribute.AttributeGroup").Include("ImageList").AsNoTracking().Where(c => c.Id == id).FirstOrDefault();

                ViewData["SideCategoryTrees"] = GetSideCategory(product.CategoryId);
                ViewBag.Title = product.Name;

                if (product == null)
                    return HttpNotFound();
            }

            return View("Detail", ProductForEntityToModel(product));
        }

        public ActionResult Basket()
        {
            List<BasketProductModel> basketProductModelList = new List<BasketProductModel>();

            try
            {
                List<BasketModel> basketModelList = new List<BasketModel>();

                if (Session["MyBasket"] != null)
                {
                    basketModelList = (List<BasketModel>)Session["MyBasket"];

                    if (basketModelList.Count > 0)
                    {
                        Product product;
                        for (int i = 0; i < basketModelList.Count(); i++)
                        {
                            using (DBContext db = new DBContext())
                            {
                                int id = basketModelList[i].ProductId;
                                product = new Product();
                                product = db.Product.Include("ImageList").AsNoTracking().Where(c => c.Id == id).FirstOrDefault();

                                BasketProductModel bproductModel = new BasketProductModel();

                                bproductModel.Id = product.Id;
                                bproductModel.Name = product.Name;
                                bproductModel.Price = product.Price;
                                bproductModel.UnitInStock = product.UnitInStock;
                                bproductModel.Quantity = basketModelList[i].Quantity;
                                bproductModel.Discount = 0;//TODO:Kampanyada eklenicek

                                if (product.ImageList.Count > 0)
                                    bproductModel.ImageList.Add(product.ImageList[0]);

                                basketProductModelList.Add(bproductModel);
                            }
                        }

                        ViewBag.TotalQuantity = ProductInBasket().ToString();
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            ViewBag.TESTO = GetListFromDatabase();

            return View(basketProductModelList);
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

            ImageModel img;

            foreach (var item in entity.ImageList)
            {
                img = new ImageModel();

                img.Id = item.Id;
                img.ProductId = item.ProductId;
                img.FilePath = item.FilePath;
                img.Ranking = item.Ranking;

                productModel.ImageList.Add(img);
            }

            productModel.ProductToAttributeList = entity.ProductToAttributeList.OrderBy(a => a.Attribute.AttributeGroupId).ToList();

            //foreach (var item in entity.ProductToAttributeList)
            //{
            //    productModel.ProductToAttributeList.Add(item);
            //}

            

            return productModel;
        }

        static List<Category> GetListFromDatabase()
        {

            List<Category> allCategory;
            using (DBContext db = new DBContext())
            {
                allCategory = db.Category.Include("ParentCategory").Include("SubCategories").Where(c => c.Deleted == false).ToList();
            }

            return allCategory;
        }

        public string GetSideCategory(int catID)
        {
            string result = "<ul class=\"breadcrumb\">";
            result += "<li>" + " <a href=\"@Url.Action(\"Index\",\"Home\")\"> Ana Sayfa </a></li>";
            result += GetParentCategoryTree(string.Empty, catID);
            result +="</ul>";
            return result;
        }

        public string GetParentCategoryTree(string value,int categoryId)
        {
            string result = value;
            Category category = GetCategory(categoryId);
            if (category.ParentId != null)
                result += GetParentCategoryTree(result, category.ParentCategory.Id) + "<li>" + " <a href=\"@Url.Action(\"Index\", \"Product\", new { id = " + category.Id + "})\">" + category.Name + "</a></li>";
            else
                result += "<li>" + " <a href=\"@Url.Action(\"Index\", \"Product\", new { id = " + category.Id + "})\"> " + category.Name + "</a></li>";
            return result;
        }
        public Category GetCategory(int catId)
        {
            Category category;

            using (DBContext db = new DBContext())
               category = db.Category.Include("ParentCategory").Where(c => c.Id == catId).FirstOrDefault();
            
            return category;
        }

        public ActionResult GetAllCategory(int id)
        {

            Product product;

            using (DBContext db = new DBContext())
            {
                product = db.Product.Include("Category").Include("ProductToAttributeList").Include("ProductToAttributeList.Attribute").Include("ProductToAttributeList.Attribute.AttributeGroup").Include("ImageList").AsNoTracking().Where(c => c.Id == id).FirstOrDefault();

                ViewBag.Title = product.Name;

                if (product == null)
                    return HttpNotFound();
            }

            return View("Detail", ProductForEntityToModel(product));
        }

        public ActionResult AddToCart(int ProductId, int Quantity)
        {
            AddSessionProductToBasket(ProductId, Quantity);
           //TODO: veri tabanı kaydı ve cerez eklenicek
            TotalPrice();
            return Content(ProductInBasket().ToString());
        }

        public ActionResult RemoveToCart(int ProductId)
        {
            RemoveSessionProductToBasket(ProductId);
            TotalPrice();
            return Content(ProductInBasket().ToString());
        }

        public void AddSessionProductToBasket(int ProductId, int Quantity)
        {
            try
            {
                List<BasketModel> basketModelList = new List<BasketModel>();

                if (Session["MyBasket"] != null)
                {
                    basketModelList = (List<BasketModel>)Session["MyBasket"];

                }

                bool productCheck = false;
                int productIndex = 0;

                for (int i = 0; i < basketModelList.Count(); i++)
                {
                    if (ProductId == basketModelList[i].ProductId)
                    {
                        productIndex = i;
                        productCheck = true;
                        break;
                    }
                }

                if (productCheck == false)
                {
                    BasketModel basketModel = new BasketModel();
                    basketModel.ProductId = ProductId;
                    basketModel.Quantity = Quantity;

                    basketModelList.Add(basketModel);
                }
                else
                {
                    if (basketModelList[productIndex].ProductId == ProductId)
                        basketModelList[productIndex].Quantity += Quantity;
                }

                Session["MyBasket"] = basketModelList;
            }
            catch (Exception ex)
            {
                TempData.Remove("ResultMessage");
                TempData["ResultFaileMessage"] = "Sepete ekleme işlemi başarısız Tekrar deneyin. \n " + ex.Message;
            }
        }

        public void RemoveSessionProductToBasket(int ProductId)
        {
            try
            {
                List<BasketModel> basketModelList = new List<BasketModel>();

                if (Session["MyBasket"] != null)
                {
                    basketModelList = (List<BasketModel>)Session["MyBasket"];

                }
                
                for (int i = 0; i < basketModelList.Count(); i++)
                {
                    if (ProductId == basketModelList[i].ProductId)
                    {
                        basketModelList.RemoveAt(i);
                        break;
                    }
                }

                Session["MyBasket"] = basketModelList;
            }
            catch (Exception ex)
            {
                TempData.Remove("ResultMessage");
                TempData["ResultFaileMessage"] = "Sepete Silme işlemi başarısız Tekrar deneyin. \n " + ex.Message;
            }
        }

        public void ClearBasket()
        {
            try
            {
                Session["MyBasket"] = null;
            }
            catch (Exception ex)
            {
                TempData.Remove("ResultMessage");
                TempData["ResultFaileMessage"] = "Sepeti temizleme işlemi başarısız Tekrar deneyin. \n " + ex.Message;
            }
        }

        public int ProductInBasket()
        {
            int productInBasket = 0;
            try
            {
                List<BasketModel> basketModelList = new List<BasketModel>();

                if (Session["MyBasket"] != null)
                    basketModelList = (List<BasketModel>)Session["MyBasket"];
                else
                    return 0;

                foreach (BasketModel item in basketModelList)
                {
                    productInBasket += item.Quantity;
                }

                TempData["MyBacketCount"] = productInBasket;
            }
            catch (Exception ex)
            {
                TempData.Remove("ResultMessage");
                TempData["ResultFaileMessage"] = "Sepete Sayma işlemi başarısız Tekrar deneyin. \n " + ex.Message;
            }

            return productInBasket;
        }

        public decimal TotalPrice()
        {
            decimal totalPrice = 0;
            try
            {
                List<BasketModel> basketModelList = new List<BasketModel>();

                if (Session["MyBasket"] != null)
                    basketModelList = (List<BasketModel>)Session["MyBasket"];
                else
                    return 0;

                Product product;
                foreach (BasketModel item in basketModelList)
                {
                    int id = item.ProductId;

                    product = new Product();

                    using (DBContext db = new DBContext())
                        product = db.Product.Include("ImageList").AsNoTracking().Where(c => c.Id == id).FirstOrDefault();

                    totalPrice += item.Quantity * product.Price;
                }

                TempData["TotalPrice"] = totalPrice;
            }
            catch (Exception ex)
            {
                TempData.Remove("ResultMessage");
                TempData["ResultFaileMessage"] = "Sepete Sayma işlemi başarısız Tekrar deneyin. \n " + ex.Message;
            }

            return totalPrice;
        }
    }
}