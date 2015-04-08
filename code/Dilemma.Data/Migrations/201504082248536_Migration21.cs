using Dilemma.Data.EntityFramework;

namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration21 : DbMigration
    {
        public override void Up()
        {
            Sql("TRUNCATE TABLE dbo.Notification");

            AddColumn("dbo.Notification", "NotificationTarget", c => c.Int(nullable: false));
            AddColumn("dbo.Notification", "Followup_FollowupId", c => c.Int());
            CreateIndex("dbo.Notification", "Followup_FollowupId");
            AddForeignKey("dbo.Notification", "Followup_FollowupId", "dbo.Followup", "FollowupId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notification", "Followup_FollowupId", "dbo.Followup");
            DropIndex("dbo.Notification", new[] { "Followup_FollowupId" });
            DropColumn("dbo.Notification", "Followup_FollowupId");
            DropColumn("dbo.Notification", "NotificationTarget");
        }
    }
}
