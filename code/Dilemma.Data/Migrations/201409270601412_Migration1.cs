namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Moderation",
                c => new
                    {
                        ModerationId = c.Int(nullable: false, identity: true),
                        ModerationFor = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        Answer_AnswerId = c.Int(),
                        ForUser_UserId = c.Int(nullable: false),
                        Question_QuestionId = c.Int(),
                    })
                .PrimaryKey(t => t.ModerationId)
                .ForeignKey("dbo.Answer", t => t.Answer_AnswerId)
                .ForeignKey("dbo.User", t => t.ForUser_UserId)
                .ForeignKey("dbo.Question", t => t.Question_QuestionId)
                .Index(t => t.Answer_AnswerId)
                .Index(t => t.ForUser_UserId)
                .Index(t => t.Question_QuestionId);
            
            CreateTable(
                "dbo.ModerationEntry",
                c => new
                    {
                        ModerationEntryId = c.Int(nullable: false, identity: true),
                        EntryType = c.Int(nullable: false),
                        Message = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        Moderation_ModerationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ModerationEntryId)
                .ForeignKey("dbo.Moderation", t => t.Moderation_ModerationId, cascadeDelete: true)
                .Index(t => t.Moderation_ModerationId);
            
            AddColumn("dbo.Answer", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Question", "IsApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Moderation", "Question_QuestionId", "dbo.Question");
            DropForeignKey("dbo.ModerationEntry", "Moderation_ModerationId", "dbo.Moderation");
            DropForeignKey("dbo.Moderation", "ForUser_UserId", "dbo.User");
            DropForeignKey("dbo.Moderation", "Answer_AnswerId", "dbo.Answer");
            DropIndex("dbo.ModerationEntry", new[] { "Moderation_ModerationId" });
            DropIndex("dbo.Moderation", new[] { "Question_QuestionId" });
            DropIndex("dbo.Moderation", new[] { "ForUser_UserId" });
            DropIndex("dbo.Moderation", new[] { "Answer_AnswerId" });
            DropColumn("dbo.Question", "IsApproved");
            DropColumn("dbo.Answer", "IsApproved");
            DropTable("dbo.ModerationEntry");
            DropTable("dbo.Moderation");
        }
    }
}
