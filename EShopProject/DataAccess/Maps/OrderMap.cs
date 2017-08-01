using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Maps
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            HasKey(o => o.Id);

            HasRequired(o => o.User)
                .WithMany(u => u.OrderList)
                .HasForeignKey(o => o.UserId)
                .WillCascadeOnDelete(false);


            HasRequired(o => o.Address)
                .WithMany(a => a.OrderList)
                .HasForeignKey(o => o.AddressId)
                .WillCascadeOnDelete(false);

            Property(o => o.OrderState)
                .HasColumnType("int")
                .IsRequired();

            Property(o => o.OrderDate)
                .HasColumnType("Date")
                .IsRequired();

            Property(o => o.DeliveryDate)
                .HasColumnType("Date")
                .IsOptional();

            Property(o => o.ShippedDate)
                .HasColumnType("Date")
                .IsOptional();

            Property(o => o.Deleted)
                .HasColumnType("bit");
        }
    }
}