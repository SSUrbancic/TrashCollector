namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFKOfUserToCustomerTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "userID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Customers", "userID");
            AddForeignKey("dbo.Customers", "userID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "userID", "dbo.AspNetUsers");
            DropIndex("dbo.Customers", new[] { "userID" });
            AlterColumn("dbo.Customers", "userID", c => c.String());
        }
    }
}
