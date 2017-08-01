using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Maps
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            HasKey(c => c.Id);

            Property(u => u.EmailAddress)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(c => c.Password)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .IsRequired();

            Property(u => u.CreateDate)
                .HasColumnType("datetime")
                .IsRequired();

            Property(u => u.LastLoginDate)
                .HasColumnType("datetime")
                .IsOptional();

            Property(u => u.Deleted)
                .HasColumnType("bit");

        }
    }
}