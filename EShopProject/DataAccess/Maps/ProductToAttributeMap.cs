using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Maps
{
    public class ProductToAttributeMap :EntityTypeConfiguration<ProductToAttribute>
    {
        public ProductToAttributeMap()
        {
            HasKey(pta => pta.Id);

            HasRequired(pta => pta.Product)
                .WithMany(p => p.ProductToAttributeList)
                .HasForeignKey(pta => pta.ProductId)
                .WillCascadeOnDelete(false);

            HasRequired(pta => pta.Attribute)
                .WithMany(a => a.ProductToAttributeList)
                .HasForeignKey(pta => pta.AttributeId)
                .WillCascadeOnDelete(false);

            Property(i => i.Value)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsRequired();
        }
    }
}