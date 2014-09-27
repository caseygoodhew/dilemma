namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Moderation", new[] { "MostRecentEntry_ModerationEntryId" });
            AlterColumn("dbo.Moderation", "MostRecentEntry_ModerationEntryId", c => c.Int());
            CreateIndex("dbo.Moderation", "MostRecentEntry_ModerationEntryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Moderation", new[] { "MostRecentEntry_ModerationEntryId" });
            AlterColumn("dbo.Moderation", "MostRecentEntry_ModerationEntryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Moderation", "MostRecentEntry_ModerationEntryId");
        }
    }
}
