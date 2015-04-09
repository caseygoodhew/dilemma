namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration22 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notification", "Moderation_ModerationId", "dbo.Moderation");
            DropIndex("dbo.Notification", new[] { "Moderation_ModerationId" });
            DropColumn("dbo.Notification", "Moderation_ModerationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notification", "Moderation_ModerationId", c => c.Int());
            CreateIndex("dbo.Notification", "Moderation_ModerationId");
            AddForeignKey("dbo.Notification", "Moderation_ModerationId", "dbo.Moderation", "ModerationId");
        }
    }
}
