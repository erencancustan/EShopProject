using EShopProject.DataAccess.Entity;
using EShopProject.DataAccess.Maps;
using System.Data.Entity;

namespace EShopProject
{
    public class DBContext : DbContext
    {
        public DBContext() : base("EShopProjectDB")
        { }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<DataAccess.Entity.Attribute> Attribute { get; set; }
        public virtual DbSet<AttributeGroup> AttributeGroup { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductToAttribute> ProductToAttribute { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new AttributeMap());
            modelBuilder.Configurations.Add(new AttributeGroupMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new ImageMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new OrderDetailMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new ProductToAttributeMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}