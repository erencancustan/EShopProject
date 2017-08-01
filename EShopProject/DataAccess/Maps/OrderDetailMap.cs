using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Maps
{
    public class OrderDetailMap : EntityTypeConfiguration<OrderDetail>
    {
        public OrderDetailMap()
        {
            HasKey(od => od.Id);

            HasRequired(od => od.Order)
                .WithMany(o => o.OrderDetailList)
                .HasForeignKey(od => od.OrderId)
                .WillCascadeOnDelete(false);

            HasRequired(od => od.Product)
                .WithMany(p => p.OrderDetailList)
                .HasForeignKey(od => od.ProductId)
                .WillCascadeOnDelete(false);

            Property(od => od.UnitPrice)
                .HasColumnType("money")
                .IsRequired();

            Property(od => od.Quantity)
                .HasColumnType("int")
                .IsRequired();

            Property(od => od.Discount)
                .HasColumnType("float")
                .IsOptional();
        }
    }
}