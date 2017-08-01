using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Entity
{
    public class Product
    {
        public Product()
        {
            ImageList = new List<Image>();
            ProductToAttributeList = new List<ProductToAttribute>();
        }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public int UnitInStock { get; set; }
        public bool Deleted { get; set; }

        public virtual Category Category { get; set; }
        public virtual List<Image> ImageList { get; private set; }
        public virtual List<ProductToAttribute> ProductToAttributeList { get; private set; }
        public virtual List<OrderDetail> OrderDetailList { get; private set; }
    }
}