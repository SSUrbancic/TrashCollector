namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecreatedTrashPickUp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrashPickUps", "CustomerID", c => c.Int(nullable: false));
            CreateIndex("dbo.TrashPickUps", "CustomerID");
            AddForeignKey("dbo.TrashPickUps", "CustomerID", "dbo.Customers", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrashPickUps", "CustomerID", "dbo.Customers");
            DropIndex("dbo.TrashPickUps", new[] { "CustomerID" });
            DropColumn("dbo.TrashPickUps", "CustomerID");
        }
    }
}
