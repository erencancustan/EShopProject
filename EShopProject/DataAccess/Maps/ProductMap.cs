using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Maps
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            HasKey(p => p.Id);

            Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(p => p.Description)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsRequired();

            Property(p => p.Price)
                .HasColumnType("money")
                .IsRequired();

            Property(p => p.UnitInStock)
                .HasColumnType("int")
                .IsRequired();

            Property(p => p.Deleted)
                .HasColumnType("bit");
            
            HasRequired(p => p.Category)
                .WithMany(c => c.ProductList)
                .HasForeignKey(p => p.CategoryId)
                .WillCascadeOnDelete(false);
        }
    }
}