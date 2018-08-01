namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionID = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        PickUpID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionID)
                .ForeignKey("dbo.TrashPickUps", t => t.PickUpID, cascadeDelete: true)
                .Index(t => t.PickUpID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "PickUpID", "dbo.TrashPickUps");
            DropIndex("dbo.Transactions", new[] { "PickUpID" });
            DropTable("dbo.Transactions");
        }
    }
}
