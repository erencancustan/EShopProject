using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Entity
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PostAddress { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool Deleted { get; set; }

        public virtual List<Customer> UserList { get; private set; }
        public virtual List<Order> OrderList { get; private set; }
    }
}