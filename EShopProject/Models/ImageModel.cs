using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string FilePath { get; set; }
        public int Ranking { get; set; }

        public virtual ProductModel Product { get; set; }
    }
}