using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Entity
{
    public class Image
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string FilePath { get; set; }
        public int Ranking { get; set; }
        public bool Deleted { get; set; }

        public virtual Product Product { get; set; }
    }
}