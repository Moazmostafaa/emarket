namespace emarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixPrimaryKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Id", "dbo.Carts");
            DropForeignKey("dbo.Products", "Id", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "Id" });
            DropPrimaryKey("dbo.Carts");
            DropPrimaryKey("dbo.Products");
            AlterColumn("dbo.Carts", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.Products", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Carts", "ProductId");
            AddPrimaryKey("dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Products");
            DropPrimaryKey("dbo.Carts");
            AlterColumn("dbo.Products", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Carts", "ProductId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Products", "Id");
            AddPrimaryKey("dbo.Carts", "ProductId");
            CreateIndex("dbo.Products", "Id");
            AddForeignKey("dbo.Products", "Id", "dbo.Categories", "Id");
            AddForeignKey("dbo.Products", "Id", "dbo.Carts", "ProductId");
        }
    }
}
