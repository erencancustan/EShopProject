using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
            ImageList = new List<ImageModel>();
            ProductToAttributeList = new List<ProductToAttribute>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public int UnitInStock { get; set; }
        public bool Deleted { get; set; }
        private ImageModel ImageTemp { get; set; }

        public ImageModel Image
        {
            get
            {
                if (ImageList.Count < 1)
                    return null;

                foreach (ImageModel img in ImageList)
                    if (img.Ranking == 1)
                        ImageTemp = img;

                if (ImageTemp == null)
                    ImageTemp = ImageList[0];

                return ImageTemp;
            }
        }
        public virtual CategoryModel Category { get; set; }
        public virtual List<ImageModel> ImageList { get; set; }
        public virtual List<ProductToAttribute> ProductToAttributeList { get;  set; }
    }
}