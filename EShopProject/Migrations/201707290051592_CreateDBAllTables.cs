namespace EShopProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDBAllTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30, unicode: false),
                        PostAddress = c.String(nullable: false, maxLength: 500, unicode: false),
                        City = c.String(nullable: false, maxLength: 30, unicode: false),
                        Region = c.String(nullable: false, maxLength: 30, unicode: false),
                        PostalCode = c.String(nullable: false, maxLength: 10, unicode: false),
                        Country = c.String(nullable: false, maxLength: 30, unicode: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false, storeType: "date"),
                        DeliveryDate = c.DateTime(storeType: "date"),
                        ShippedDate = c.DateTime(storeType: "date"),
                        OrderState = c.Int(nullable: false),
                        Cancellation = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.Customers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, storeType: "money"),
                        Quantity = c.Int(nullable: false),
                        Discount = c.Double(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        Description = c.String(nullable: false, maxLength: 500, unicode: false),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        UnitInStock = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30, unicode: false),
                        Description = c.String(nullable: false, maxLength: 1000, unicode: false),
                        Deleted = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        FilePath = c.String(nullable: false, maxLength: 100, unicode: false),
                        Ranking = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductToAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        AttributeId = c.Int(nullable: false),
                        Value = c.String(nullable: false, maxLength: 500, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Attributes", t => t.AttributeId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.AttributeId);
            
            CreateTable(
                "dbo.Attributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttributeGroupId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        Description = c.String(nullable: false, maxLength: 200, unicode: false),
                        AttributeValueType = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AttributeGroups", t => t.AttributeGroupId)
                .Index(t => t.AttributeGroupId);
            
            CreateTable(
                "dbo.AttributeGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30, unicode: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 30, unicode: false),
                        LastName = c.String(nullable: false, maxLength: 30, unicode: false),
                        Gender = c.Int(nullable: false),
                        BirthOfDate = c.DateTime(nullable: false, storeType: "date"),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailAddress = c.String(nullable: false, maxLength: 50, unicode: false),
                        Password = c.String(nullable: false, maxLength: 30, unicode: false),
                        MyAuthorization = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        LastLoginDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserTOAddress",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.AddressId })
                .ForeignKey("dbo.Customers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.AddressId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "Id", "dbo.Users");
            DropForeignKey("dbo.UserTOAddress", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.UserTOAddress", "UserId", "dbo.Customers");
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductToAttributes", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductToAttributes", "AttributeId", "dbo.Attributes");
            DropForeignKey("dbo.Attributes", "AttributeGroupId", "dbo.AttributeGroups");
            DropForeignKey("dbo.Images", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories", "ParentId", "dbo.Categories");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "AddressId", "dbo.Addresses");
            DropIndex("dbo.UserTOAddress", new[] { "AddressId" });
            DropIndex("dbo.UserTOAddress", new[] { "UserId" });
            DropIndex("dbo.Customers", new[] { "Id" });
            DropIndex("dbo.Attributes", new[] { "AttributeGroupId" });
            DropIndex("dbo.ProductToAttributes", new[] { "AttributeId" });
            DropIndex("dbo.ProductToAttributes", new[] { "ProductId" });
            DropIndex("dbo.Images", new[] { "ProductId" });
            DropIndex("dbo.Categories", new[] { "ParentId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "AddressId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropTable("dbo.UserTOAddress");
            DropTable("dbo.Users");
            DropTable("dbo.Customers");
            DropTable("dbo.AttributeGroups");
            DropTable("dbo.Attributes");
            DropTable("dbo.ProductToAttributes");
            DropTable("dbo.Images");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Orders");
            DropTable("dbo.Addresses");
        }
    }
}
