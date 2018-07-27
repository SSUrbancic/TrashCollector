namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserIDPropertyToEmployeeAndCustomerModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "userID", c => c.String());
            AddColumn("dbo.Employees", "userID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "userID");
            DropColumn("dbo.Customers", "userID");
        }
    }
}
