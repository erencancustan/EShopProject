using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Maps
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            HasKey(c => c.Id);

            Property(c => c.Name)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .IsRequired();

            Property(c => c.PostAddress)
                .HasColumnType("varchar")
                .HasMaxLength(500)
                .IsRequired();

            Property(c => c.City)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .IsRequired();

            Property(c => c.Region)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .IsRequired();

            Property(c => c.PostalCode)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsRequired();

            Property(c => c.Country)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .IsRequired();

            Property(c => c.Deleted)
                .HasColumnType("bit");
        }
    }
}