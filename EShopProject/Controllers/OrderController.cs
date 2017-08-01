using EShopProject.DataAccess.Entity;
using EShopProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShopProject.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OrderDetail(int Id)
        {

            ViewBag.Title = "Sipariş Detayı";
            if (Id == 0)
                RedirectToAction("Indezx", "Home");

            User sessionUser;

            if (Session["Member"] == null)
            {
                return RedirectToAction("Index", "UserOperations");
            }
            else
            {
                sessionUser = (User)Session["Member"];

                if (sessionUser.Customer == null)
                    return RedirectToAction("CustomerCreate", "UserOperations");
            }

            Order order;
            using (DBContext db = new DBContext())
            {
                order = db.Order.Include("OrderDetailList").Include("OrderDetailList.Product").Include("OrderDetailList.Product.ImageList").Include("Address").AsNoTracking().FirstOrDefault(o => o.Id == Id && o.UserId == sessionUser.Id);

                if (order == null)
                {
                    return RedirectToAction("Index", "UserOperations");
                }
            }

            TotalPrice(order);

            return View("OrderDetail", order);
        }

        public ActionResult MyOrderList()
        {
            User sessionUser;

            if (Session["Member"] == null)
            {
                return RedirectToAction("Index", "UserOperations");
            }
            else
            {
                sessionUser = (User)Session["Member"];

                if (sessionUser.Customer == null)
                    return RedirectToAction("CustomerCreate", "UserOperations");
            }

            List<DetailOrderListModel> detailOrderListModel = new List<DetailOrderListModel>();

            using (DBContext db = new DBContext())
            {
                List<Order> orderList;
                orderList = db.Order.Include("OrderDetailList").Include("Address").AsNoTracking().Where(o => o.UserId == sessionUser.Id).ToList();

                if (orderList == null)
                {
                    return RedirectToAction("Index", "UserOperations");
                }

                DetailOrderListModel detailOrderListItem;

                foreach (Order o in orderList)
                {
                    detailOrderListItem = new DetailOrderListModel();
                    detailOrderListItem.Id = o.Id;
                    detailOrderListItem.OrderDate = o.OrderDate;
                    detailOrderListItem.TotalPrice = TotalPrice(o);
                    detailOrderListItem.OrderState = o.OrderState;
                    detailOrderListItem.AddressName = o.Address.Name;

                    detailOrderListModel.Add(detailOrderListItem);
                }
            }
            return View("MyOrderList", detailOrderListModel);
        }

        [HttpPost]
        public ActionResult SubmitBasketToOrder()
        {
            try
            {

                User sessionUser;
                if (Session["Member"] == null)
                {
                    return RedirectToAction("Index", "UserOperations");
                }
                else
                {
                    sessionUser = (User)Session["Member"];

                    if (sessionUser.Customer == null)
                        return RedirectToAction("CustomerCreate", "UserOperations");
                }


                List<BasketModel> basketModeList = new List<BasketModel>();

                BasketModel model;

                string stringId = Request.Form["Id"];
                string stringQuantity = Request.Form["Quantity"];

                string[] arrayId = stringId.Split(',');
                string[] arrayQuantity = stringQuantity.Split(',');


                for (int i = 0; i < arrayId.Length; i++)
                {
                    model = new BasketModel();
                    model.ProductId = Convert.ToInt32(arrayId[i]);
                    model.Quantity = Convert.ToInt32(arrayQuantity[i]);

                    basketModeList.Add(model);
                }
                Order order = new Order();

                order.UserId = sessionUser.Id;
                order.AddressId = 1;//TODO;
                order.OrderDate = DateTime.Now;
                order.OrderState = OrderState.Hazırlanıyor;

                order.OrderDetailList = new List<OrderDetail>();

                OrderDetail orderDetail;
                using (DBContext db = new DBContext())
                {
                    for (int i = 0; i < arrayId.Length; i++)
                    {
                        orderDetail = new OrderDetail();
                        orderDetail.ProductId = Convert.ToInt32(arrayId[i]);
                        orderDetail.Quantity = Convert.ToInt32(arrayQuantity[i]);

                        Product product = db.Product.AsNoTracking().FirstOrDefault(p => p.Id == orderDetail.ProductId);

                        if (product == null)
                        {
                            TempData.Remove("ResultMessage");
                            TempData["ResultFaileMessage"] = "Ürününlerde bir hata oluştu";
                            return RedirectToAction("Basket", "Product");
                        }
                        if (product.UnitInStock < orderDetail.Quantity)
                        {
                            TempData.Remove("ResultMessage");
                            TempData["ResultFaileMessage"] = product.Name + " ürününden stokta istenilen adet bulunmamaktadır.\n Stok Adeti : " + product.UnitInStock;
                            return RedirectToAction("Basket", "Product");
                        }
                        orderDetail.UnitPrice = product.Price;
                        orderDetail.Discount = 0;

                        order.OrderDetailList.Add(orderDetail);
                    }

                    if (order.Id == 0)
                    {
                        db.Entry(order).State = System.Data.Entity.EntityState.Added;
                        TempData["ResultMessage"] = " Siparişiz Başarıyla alınmıştır.";
                    }

                    db.SaveChanges();// Ürün Fiyatı Adeti Düşürülür.
                    ClearBasket();
                }
                return View("Home", "Index");
            }
            catch (Exception ex)
            {
                TempData.Remove("ResultMessage");
                TempData["ResultFaileMessage"] = "Sipariş aşamasında hata meydana geldi. Lütfen tekrar deneyiniz.\n " + ex.Message;
                return RedirectToAction("Basket", "Product");
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

        public decimal TotalPrice(Order order)
        {
            decimal totalPrice = 0;

            foreach (OrderDetail item in order.OrderDetailList)
            {
                totalPrice += item.UnitPrice * item.Quantity;
            }

            ViewData["TotalPrice"] = totalPrice.ToString("F");

            return totalPrice;
        }
    }
}