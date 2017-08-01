using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public DateTime OrderDate{ get; set; }
        public Nullable<DateTime> DeliveryDate { get; set; }
        public Nullable<DateTime> ShippedDate { get; set; }
        public OrderState OrderState { get; set; }
        public bool Cancellation { get; set; }
        public bool Deleted { get; set; }

        public virtual Customer User { get; set; }
        public virtual Address Address { get; set; }
        public virtual List<OrderDetail> OrderDetailList { get; set; }
    }
}