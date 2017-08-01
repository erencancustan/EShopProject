using System;
using System.Collections.Generic;

namespace EShopProject.Models
{
    public class CategoryModel
    {
        public CategoryModel()
        {
            ProductList = new List<ProductModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> ParentId { get; set; }

        public virtual CategoryModel ParentCategory { get; set; }
        public virtual List<CategoryModel> SubCategories { get; private set; }
        public virtual List<ProductModel> ProductList { get; private set; }
    }
}