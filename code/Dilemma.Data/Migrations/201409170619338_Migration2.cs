namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        NotificationType = c.Int(nullable: false),
                        NotificationTypeKey = c.String(nullable: false),
                        Controller = c.String(nullable: false),
                        Action = c.String(nullable: false),
                        RouteDataKey = c.String(),
                        RouteDataValue = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        ActionedDateTime = c.DateTime(),
                        ForUser_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.User", t => t.ForUser_UserId)
                .Index(t => t.ForUser_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notification", "ForUser_UserId", "dbo.User");
            DropIndex("dbo.Notification", new[] { "ForUser_UserId" });
            DropTable("dbo.Notification");
        }
    }
}
