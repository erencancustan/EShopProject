using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Maps
{
    public class ImageMap: EntityTypeConfiguration<Image>
    {
        public ImageMap()
        {
            HasKey(i => i.Id);

            Property(i => i.FilePath)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            Property(i => i.Ranking)
                .HasColumnType("int")
                .IsRequired();

            Property(i => i.Deleted)
               .HasColumnType("bit");

            HasRequired(i => i.Product)
                .WithMany(p => p.ImageList)
                .HasForeignKey(i => i.ProductId)
                .WillCascadeOnDelete(false);
        }
    }
}