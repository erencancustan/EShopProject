using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Maps
{
    public class AttributeGroupMap :EntityTypeConfiguration<AttributeGroup>
    {
        public AttributeGroupMap()
        {
            HasKey(c => c.Id);

            Property(c => c.Name)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .IsRequired();

            Property(c => c.Deleted)
                .HasColumnType("bit");
        }
    }
}