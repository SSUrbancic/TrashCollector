namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TrashPickUps", "date", c => c.String());
            AlterColumn("dbo.TrashPickUps", "dayOfWeek", c => c.String());
            DropColumn("dbo.TrashPickUps", "customerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TrashPickUps", "customerID", c => c.Int(nullable: false));
            AlterColumn("dbo.TrashPickUps", "dayOfWeek", c => c.Int(nullable: false));
            AlterColumn("dbo.TrashPickUps", "date", c => c.DateTime(nullable: false));
        }
    }
}
