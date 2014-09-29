namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answer",
                c => new
                    {
                        AnswerId = c.Int(nullable: false, identity: true),
                        Text = c.String(maxLength: 2000),
                        CreatedDateTime = c.DateTime(nullable: false),
                        AnswerType = c.Int(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        Question_QuestionId = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerId)
                .ForeignKey("dbo.Question", t => t.Question_QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_UserId)
                .Index(t => t.Question_QuestionId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 2000),
                        CreatedDateTime = c.DateTime(nullable: false),
                        ClosesDateTime = c.DateTime(nullable: false),
                        MaxAnswers = c.Int(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        Category_CategoryId = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.Category", t => t.Category_CategoryId)
                .ForeignKey("dbo.User", t => t.User_UserId)
                .Index(t => t.Category_CategoryId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UserType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.DevelopmentUser",
                c => new
                    {
                        DevelopmentUserId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DevelopmentUserId);
            
            CreateTable(
                "dbo.ModerationEntry",
                c => new
                    {
                        ModerationEntryId = c.Int(nullable: false, identity: true),
                        EntryType = c.Int(nullable: false),
                        Message = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        AddedByUser_UserId = c.Int(nullable: false),
                        Moderation_ModerationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ModerationEntryId)
                .ForeignKey("dbo.User", t => t.AddedByUser_UserId)
                .ForeignKey("dbo.Moderation", t => t.Moderation_ModerationId, cascadeDelete: true)
                .Index(t => t.AddedByUser_UserId)
                .Index(t => t.Moderation_ModerationId);
            
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
                "dbo.Notification",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        NotificationType = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        ActionedDateTime = c.DateTime(),
                        Answer_AnswerId = c.Int(),
                        ForUser_UserId = c.Int(nullable: false),
                        Moderation_ModerationId = c.Int(),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.Answer", t => t.Answer_AnswerId)
                .ForeignKey("dbo.User", t => t.ForUser_UserId)
                .ForeignKey("dbo.Moderation", t => t.Moderation_ModerationId)
                .Index(t => t.Answer_AnswerId)
                .Index(t => t.ForUser_UserId)
                .Index(t => t.Moderation_ModerationId);
            
            CreateTable(
                "dbo.SystemConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaxAnswers = c.Int(nullable: false),
                        QuestionLifetime = c.Int(nullable: false),
                        SystemEnvironment = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notification", "Moderation_ModerationId", "dbo.Moderation");
            DropForeignKey("dbo.Notification", "ForUser_UserId", "dbo.User");
            DropForeignKey("dbo.Notification", "Answer_AnswerId", "dbo.Answer");
            DropForeignKey("dbo.Moderation", "Question_QuestionId", "dbo.Question");
            DropForeignKey("dbo.ModerationEntry", "Moderation_ModerationId", "dbo.Moderation");
            DropForeignKey("dbo.Moderation", "ForUser_UserId", "dbo.User");
            DropForeignKey("dbo.Moderation", "Answer_AnswerId", "dbo.Answer");
            DropForeignKey("dbo.ModerationEntry", "AddedByUser_UserId", "dbo.User");
            DropForeignKey("dbo.Answer", "User_UserId", "dbo.User");
            DropForeignKey("dbo.Question", "User_UserId", "dbo.User");
            DropForeignKey("dbo.Question", "Category_CategoryId", "dbo.Category");
            DropForeignKey("dbo.Answer", "Question_QuestionId", "dbo.Question");
            DropIndex("dbo.Notification", new[] { "Moderation_ModerationId" });
            DropIndex("dbo.Notification", new[] { "ForUser_UserId" });
            DropIndex("dbo.Notification", new[] { "Answer_AnswerId" });
            DropIndex("dbo.Moderation", new[] { "Question_QuestionId" });
            DropIndex("dbo.Moderation", new[] { "ForUser_UserId" });
            DropIndex("dbo.Moderation", new[] { "Answer_AnswerId" });
            DropIndex("dbo.ModerationEntry", new[] { "Moderation_ModerationId" });
            DropIndex("dbo.ModerationEntry", new[] { "AddedByUser_UserId" });
            DropIndex("dbo.Question", new[] { "User_UserId" });
            DropIndex("dbo.Question", new[] { "Category_CategoryId" });
            DropIndex("dbo.Answer", new[] { "User_UserId" });
            DropIndex("dbo.Answer", new[] { "Question_QuestionId" });
            DropTable("dbo.SystemConfiguration");
            DropTable("dbo.Notification");
            DropTable("dbo.Moderation");
            DropTable("dbo.ModerationEntry");
            DropTable("dbo.DevelopmentUser");
            DropTable("dbo.User");
            DropTable("dbo.Category");
            DropTable("dbo.Question");
            DropTable("dbo.Answer");
        }
    }
}
