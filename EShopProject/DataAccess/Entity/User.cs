using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public MyAuthorization MyAuthorization { get; set; }
        public DateTime CreateDate { get; set; }
        public Nullable<DateTime> LastLoginDate { get; set; }
        public bool Deleted { get; set; }

        public virtual Customer Customer { get; set; }
    }
}