namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Notification", "Controller");
            DropColumn("dbo.Notification", "Action");
            DropColumn("dbo.Notification", "RouteDataKey");
            DropColumn("dbo.Notification", "RouteDataValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notification", "RouteDataValue", c => c.String());
            AddColumn("dbo.Notification", "RouteDataKey", c => c.String());
            AddColumn("dbo.Notification", "Action", c => c.String(nullable: false));
            AddColumn("dbo.Notification", "Controller", c => c.String(nullable: false));
        }
    }
}
