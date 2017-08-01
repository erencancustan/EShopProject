using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.Models
{
    public class CustomerModel
    {
        public CustomerModel()
        {
            //User = new User();
            Address = new Address();
            AddressList = new List<Address>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthOfDate { get; set; }
          
        public Address Address { get; set; }

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