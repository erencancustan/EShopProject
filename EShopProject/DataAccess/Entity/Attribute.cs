using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Entity
{
    public class Attribute
    {
        public int Id { get; set; }
        public Nullable<int> AttributeGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AttributeValueType AttributeValueType { get; set; }
        public bool Deleted { get; set; }

        public virtual AttributeGroup AttributeGroup { get; set; }
        public virtual List<ProductToAttribute> ProductToAttributeList { get; private set; }
    }
}