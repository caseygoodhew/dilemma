namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answer", "WebPurifyAttempted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Answer", "PassedWebPurify", c => c.Boolean(nullable: false));
            AddColumn("dbo.Question", "WebPurifyAttempted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Question", "PassedWebPurify", c => c.Boolean(nullable: false));
            AddColumn("dbo.Followup", "WebPurifyAttempted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Followup", "PassedWebPurify", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Followup", "PassedWebPurify");
            DropColumn("dbo.Followup", "WebPurifyAttempted");
            DropColumn("dbo.Question", "PassedWebPurify");
            DropColumn("dbo.Question", "WebPurifyAttempted");
            DropColumn("dbo.Answer", "PassedWebPurify");
            DropColumn("dbo.Answer", "WebPurifyAttempted");
        }
    }
}
