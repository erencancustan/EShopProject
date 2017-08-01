using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Entity
{
    public class AttributeGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }

        public virtual List<Attribute> AttributeList { get; private set; }
    }
}