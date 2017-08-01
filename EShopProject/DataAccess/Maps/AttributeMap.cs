using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Maps
{
    public class AttributeMap : EntityTypeConfiguration<DataAccess.Entity.Attribute>
    {
        public AttributeMap()
        {
            HasKey(i => i.Id);

            Property(i => i.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            Property(i => i.AttributeGroupId)
                .HasColumnType("int")
                .IsOptional();

            Property(i => i.Description)
                .HasColumnType("varchar")
                .HasMaxLength(200)
                .IsRequired();

            Property(a => a.Deleted)
               .HasColumnType("bit");

            HasOptional(a => a.AttributeGroup)
                .WithMany(ag => ag.AttributeList)
                .HasForeignKey(a => a.AttributeGroupId)
                .WillCascadeOnDelete(false);
        }
    }
}