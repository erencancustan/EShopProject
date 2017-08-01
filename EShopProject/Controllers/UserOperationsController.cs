using EShopProject.DataAccess.Entity;
using EShopProject.Models;
using EShopProject.MyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EShopProject.Controllers
{
    public class UserOperationsController : Controller
    {
        // GET: UserOperations
        public ActionResult Index(SignInModel model)
        {
            ViewBag.Title = "Kullanıcı Giriş";

            if (model == null)
                model = new SignInModel();

            if (Session["Member"] != null)
            {
                using (DBContext db = new DBContext())
                {
                    User sessionUser = (User)Session["Member"];
                    User customerUser = db.User.Include("Customer").Include("Customer.AddressList").FirstOrDefault(c => c.Id == sessionUser.Id);

                    CustomerModel custormerModel;
                        
                    if (customerUser.Customer != null)
                        custormerModel = UserModelEntityForEntityToCustomerModel(customerUser);
                    else
                        custormerModel = new CustomerModel();

                    return View("CustomerCreate", custormerModel);
                }
            }

            return View(model);
        }

        public ActionResult Create(UserModel model)
        {
            ViewBag.Title = "Kayıt Ol";

            if (model == null)
                model = new UserModel();

            return View("SignUp",model);
        }

        public ActionResult CustomerCreate(CustomerModel model)
        {
            if (model == null)
                model = new CustomerModel();

            if (model.Id == 0)
                ViewBag.Title = "Üyelik Bilgileri Ekleme";
            else
                ViewBag.Title = "Üyelik Bilgileri Günceleme";

            return View("CustomerCreate", model);
        }

        [HttpPost]
        public ActionResult SignIn(SignInModel model)
        {
            using (DBContext db = new DBContext())
            {
                User user = db.User.Include("Customer").AsNoTracking().Where(s => s.EmailAddress == model.EmailAddress && !s.Deleted).FirstOrDefault();

                if (user != null)
                {
                    if (user.Password == model.Password)
                    {
                        Session["Member"] = user;
                        return RedirectToAction("Index", "Home");
                        //TODO:last login setlenicek
                    }
                }
            }
            TempData.Remove("ResultMessage");
            TempData["ResultFaileMessage"] = "Kullanıcı Adınız yada şifreniz hatalı. Lütfen tekrar deneyiniz.";
            return View("Index", model);
        }

        public ActionResult SignOut()
        {
            if ((User)Session["Member"] != null)
            {
                Session["Member"] = null;

                return RedirectToAction("Index", "UserOperations");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult GetUserInformation()
        {
            if (Session["Member"] != null)
            {
                User sessionUser = (User)Session["Member"];
                
                UserInfoModel userInfoModel = new UserInfoModel();

                userInfoModel.Id = sessionUser.Id;
                userInfoModel.EmailAddress = sessionUser.EmailAddress;
                if (sessionUser.Customer != null)
                    userInfoModel.FullName = sessionUser.Customer.FullName;
                else
                    userInfoModel.FullName = "Hoş Geldin " + sessionUser.EmailAddress.Split('@')[0];

                return PartialView("_UserLoginTopBar", userInfoModel);
            }
            else
                return PartialView("_UserTopBar");
        }

        public ActionResult GetTopBar()
        {
            if (Session["Member"] != null)
                return PartialView("_UserTopBar");
            else
                return PartialView("_NormalTopBar");
        }

        [HttpPost]
        public ActionResult Save(UserModel model)
        {
            if (!CheckModelValue(model))
            {
                return RedirectToAction("Create", model);
            }

            using (DBContext db = new DBContext())
            {
                try
                {
                    User user = UserModelForModelToEntity(model);
                    if (model.Id == 0)
                    {
                        db.Entry(user).State = System.Data.Entity.EntityState.Added;
                        TempData["ResultMessage"] = " Kullanıcı Başarıyla Eklenmiştir.";
                    }
                    else
                    {
                        db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        TempData["ResultMessage"] = " Kullanıcı Başarıyla Güncellenmiştir.";
                    }

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    TempData.Remove("ResultMessage");
                    TempData["ResultFaileMessage"] = "İşlemi başarısız tekrar deneyin.\n " + ex.Message;
                    return RedirectToAction("Create", model);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CustomerSave(CustomerModel model)
        {
            if (!CheckCustomerModelValue(model))
            {
                return RedirectToAction("CustomerCreate", model);
            }

            using (DBContext db = new DBContext())
            {
                try
                {
                    if (Session["Member"] == null)
                    {
                        TempData.Remove("ResultMessage");
                        TempData["ResultFaileMessage"] = "Kullanıcı Girişi Yapmalısınız.";
                        return RedirectToAction("Index", "UserOperations");
                    }
                    User sessionUser = (User)Session["Member"];

                    User customerUser = db.User.Include("Customer").FirstOrDefault(c=>c.Id == sessionUser.Id);

                    //if (customerUser.Customer != null)
                    //{
                    //    TempData["ResultFaileMessage"] = "Kullanıcı Girişi Yapmalısınız.";
                    //    return RedirectToAction("Index");
                    //}

                    if (customerUser == null)
                    {
                        TempData.Remove("ResultMessage");
                        TempData["ResultFaileMessage"] = "Ölümcül Hata oldu kullanıcı Uçmuş.";
                        return RedirectToAction("Index", "UserOperations");
                    }
                    
                    Customer customer = CustomerModelForModelToEntity(model);
                    customer.User = customerUser;


                    if (model.Id == 0)
                    {
                        db.Entry(customer).State = System.Data.Entity.EntityState.Added;
                        TempData["ResultMessage"] = " Üyelik Bilgileriniz Başarıyla Eklenmiştir.";
                    }
                    else
                    {
                        db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                        db.Entry(customer.AddressList).State = System.Data.Entity.EntityState.Modified;
                        db.Entry(customer.User).State = System.Data.Entity.EntityState.Modified;
                        TempData["ResultMessage"] = " Üyelik Bilgileriniz Başarıyla Güncellenmiştir.";
                    }

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    TempData.Remove("ResultMessage");
                    TempData["ResultFaileMessage"] = "İşlemi başarısız tekrar deneyin.\n " + ex.Message;
                    return RedirectToAction("CustomerCreate", model);
                }
            }
            return RedirectToAction("Index","Home");
        }

        private Customer CustomerModelForModelToEntity(CustomerModel model)
        {
            Customer customer = new Customer();

            customer.Id = model.Id;
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Gender = model.Gender;
            customer.BirthOfDate = model.BirthOfDate;

            //for (int i = 0; i < model.AddressList.Count; i++)
            //    customer.AddressList.Add(model.AddressList[i]);

            Address address = new Address();

            address.Id = model.Address.Id;
            address.Name = model.Address.Name;
            address.PostAddress = model.Address.PostAddress;
            address.City = model.Address.City;
            address.Region = model.Address.Region;
            address.Country = model.Address.Country;
            address.PostalCode = model.Address.PostalCode;

            customer.AddressList.Add(address);

            //customer.User.Id = model.User.Id;
            return customer;
        }

        private CustomerModel UserModelEntityForEntityToCustomerModel(User entity)
        {
            CustomerModel customerModel = new CustomerModel();

            customerModel.Id = entity.Id;
            customerModel.FirstName = entity.Customer.FirstName;
            customerModel.LastName = entity.Customer.LastName;
            customerModel.Gender = entity.Customer.Gender;
            customerModel.BirthOfDate = entity.Customer.BirthOfDate;

            customerModel.Address = entity.Customer.AddressList[0];

            customerModel.AddressList.Add(entity.Customer.AddressList[0]);

            //customerModel.User = entity;

            return customerModel;
        }

        private bool CheckCustomerModelValue(CustomerModel model)
        {
            bool valueCheck = false;

            if (ValueCheckHelper.EmptyCheckString(model.FirstName, model.LastName,model.Address.Name,model.Address.PostAddress, model.Address.City, model.Address.Country, model.Address.Region, model.Address.PostalCode))
            {
                TempData["ResultFaileMessage"] = "Gerekli olan değerleri doldurmalısınız.";
                return valueCheck;
            }

            DateTime birthOfDate = DateTime.Now;

            if (ValueCheckHelper.NullAndNotZeroCheckNumber(model.Gender,model.BirthOfDate) && (DateTime.TryParse(model.BirthOfDate.ToString(),out birthOfDate)))
            {
                TempData["ResultFaileMessage"] = "Hatalı Doğum tarihi değer girdiniz.";
                return valueCheck;
            }
            //model.BirthOfDate = birthOfDate;

            if (ValueCheckHelper.ValueSizeCheckString(model.FirstName, 2, 30))
            {
                TempData["ResultFaileMessage"] = "Adınızı kontrol ediniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.FirstName, 2, 30))
            {
                TempData["ResultFaileMessage"] = "Adınızı kontrol ediniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.Address.Name, 3, 30))
            {
                TempData["ResultFaileMessage"] = "Adres Adını kontrol ediniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.Address.PostAddress, 3, 500))
            {
                TempData["ResultFaileMessage"] = "Adres Detayını kontrol ediniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.Address.City, 3, 30))
            {
                TempData["ResultFaileMessage"] = "Şehir kontrol ediniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.Address.Region, 3, 30))
            {
                TempData["ResultFaileMessage"] = "Bölgeyi kontrol ediniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.Address.PostalCode, 3, 10))
            {
                TempData["ResultFaileMessage"] = "Posta Kodunu Adını kontrol ediniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.Address.Country, 3, 30))
            {
                TempData["ResultFaileMessage"] = "Ülkeyi kontrol ediniz.";
                return valueCheck;
            }
            return true;
        }

        private bool CheckModelValue(UserModel model)
        {
            bool valueCheck = false;

            if (ValueCheckHelper.EmptyCheckString(model.EmailAddress, model.Password))
            {
                TempData["ResultFaileMessage"] = "Gerekli olan değerleri doldurmalısınız.";
                return valueCheck;
            }
            if (ValueCheckHelper.NullAndNotZeroCheckNumber(model.MyAuthorization))
            {
                TempData["ResultFaileMessage"] = "Hatalı sayısal değer girdiniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.EmailAddress, 6, 50))
            {
                TempData["ResultFaileMessage"] = "Email Adresini kontrol ediniz.";
                return valueCheck;
            }
            if (ValueCheckHelper.ValueSizeCheckString(model.Password, 3, 30))
            {
                TempData["ResultFaileMessage"] = "Şifreyi kontrol ediniz.";
                return valueCheck;
            }           
            return true;
        }

        private User UserModelForModelToEntity(UserModel model)
        {
            User user = new User();

            user.Id = model.Id;
            user.EmailAddress = model.EmailAddress;
            user.Password = model.Password;
            user.CreateDate = model.CreateDate;

            if (model.LastLoginDate != null)
                user.LastLoginDate = model.LastLoginDate;

            return user;
        }
    }
}