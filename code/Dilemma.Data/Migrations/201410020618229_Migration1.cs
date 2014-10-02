namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answer", "AnswerState", c => c.Int(nullable: false));
            AddColumn("dbo.Question", "QuestionState", c => c.Int(nullable: false));
            AddColumn("dbo.ModerationEntry", "State", c => c.Int(nullable: false));
            DropColumn("dbo.Answer", "AnswerType");
            DropColumn("dbo.Answer", "IsApproved");
            DropColumn("dbo.Question", "IsApproved");
            DropColumn("dbo.ModerationEntry", "EntryType");
            DropColumn("dbo.Moderation", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Moderation", "State", c => c.Int(nullable: false));
            AddColumn("dbo.ModerationEntry", "EntryType", c => c.Int(nullable: false));
            AddColumn("dbo.Question", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Answer", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Answer", "AnswerType", c => c.Int(nullable: false));
            DropColumn("dbo.ModerationEntry", "State");
            DropColumn("dbo.Question", "QuestionState");
            DropColumn("dbo.Answer", "AnswerState");
        }
    }
}
