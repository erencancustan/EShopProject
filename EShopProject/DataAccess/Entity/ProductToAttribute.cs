using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Entity
{
    public class ProductToAttribute
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int AttributeId { get; set; }
        public string Value { get; set; }

        public virtual Product Product { get; set; }
        public virtual Attribute Attribute { get; set; }
    }
}