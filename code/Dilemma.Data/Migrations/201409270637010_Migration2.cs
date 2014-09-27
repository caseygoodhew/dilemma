namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Moderation", "MostRecentEntry_ModerationEntryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Moderation", "MostRecentEntry_ModerationEntryId");
            AddForeignKey("dbo.Moderation", "MostRecentEntry_ModerationEntryId", "dbo.ModerationEntry", "ModerationEntryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Moderation", "MostRecentEntry_ModerationEntryId", "dbo.ModerationEntry");
            DropIndex("dbo.Moderation", new[] { "MostRecentEntry_ModerationEntryId" });
            DropColumn("dbo.Moderation", "MostRecentEntry_ModerationEntryId");
        }
    }
}
