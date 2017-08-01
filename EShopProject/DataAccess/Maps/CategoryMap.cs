using EShopProject.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace EShopProject.DataAccess.Maps
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            HasKey(c => c.Id);

            Property(c => c.Name)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .IsRequired();

            Property(c => c.Description)
                .HasColumnType("varchar")
                .HasMaxLength(1000)
                .IsRequired();

            Property(c => c.Deleted)
                .HasColumnType("bit");

            HasMany(c => c.SubCategories)
            .WithOptional(c => c.ParentCategory)
            .HasForeignKey(co => co.ParentId)
            .WillCascadeOnDelete(false);
        }
    }
}