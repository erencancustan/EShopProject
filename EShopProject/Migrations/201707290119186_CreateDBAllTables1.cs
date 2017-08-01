namespace EShopProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDBAllTables1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Deleted");
        }
    }
}
