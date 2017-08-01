using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public MyAuthorization MyAuthorization { get; set; }
        public DateTime CreateDate { get; set; }
        public Nullable<DateTime> LastLoginDate { get; set; }
    }
}