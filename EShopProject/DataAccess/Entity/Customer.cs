using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Entity
{
    public class Customer
    {
        public Customer()
        {
            AddressList = new List<Address>();
            OrderList = new List<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; } 
        public DateTime BirthOfDate { get; set; }
        public bool Deleted { get; set; }


        public virtual User User { get; set; }
        public virtual List<Address> AddressList { get; set; }
        public virtual List<Order> OrderList { get; private set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}