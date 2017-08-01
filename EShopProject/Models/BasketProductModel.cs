using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.Models
{
    public class BasketProductModel
    {
        public BasketProductModel()
        {
            ImageList = new List<Image>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public int UnitInStock { get; set; }
        public int Quantity { get; set; }
        public float Discount { get; set; }
        private Image ImageTemp { get; set; }

        public Image Image
        {
            get
            {
                if (ImageList.Count < 1)
                    return null;

                foreach (Image img in ImageList)
                    if (img.Ranking == 1)
                        ImageTemp = img;

                if (ImageTemp == null)
                    ImageTemp = ImageList[0];

                return ImageTemp;
            }
        }

        public virtual List<Image> ImageList { get; set; }
    }
}