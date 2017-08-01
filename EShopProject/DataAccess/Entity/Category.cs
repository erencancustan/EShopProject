using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }
        public Nullable<int> ParentId { get; set; }

        public virtual Category ParentCategory { get; set; }
        public virtual List<Category> SubCategories { get; private set; }
        public virtual List<Product> ProductList { get; private set; }
    }
}