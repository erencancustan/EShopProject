using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Maps
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            HasKey(u => u.Id);

            Property(u => u.FirstName)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .IsRequired();

            Property(u => u.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .IsRequired();

            Property(u => u.BirthOfDate)
               .HasColumnType("Date")
               .IsRequired();

            Property(u => u.Deleted)
                .HasColumnType("bit");

            HasMany(u => u.AddressList)
                .WithMany(a => a.UserList)
                .Map(ua =>
                {
                    ua.MapLeftKey("UserId");
                    ua.MapRightKey("AddressId");
                    ua.ToTable("UserTOAddress");
                });

            HasRequired(u => u.User)
                .WithRequiredDependent(u => u.Customer);
        }
    }
}