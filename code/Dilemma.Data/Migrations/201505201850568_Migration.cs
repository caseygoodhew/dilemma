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
                        LastTouchedDateTime = c.DateTime(nullable: false),
                        AnswerState = c.Int(nullable: false),
                        Question_QuestionId = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerId)
                .ForeignKey("dbo.Question", t => t.Question_QuestionId)
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
                        ClosedDateTime = c.DateTime(),
                        MaxAnswers = c.Int(nullable: false),
                        QuestionState = c.Int(nullable: false),
                        Category_CategoryId = c.Int(nullable: false),
                        Followup_FollowupId = c.Int(),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.Category", t => t.Category_CategoryId)
                .ForeignKey("dbo.Followup", t => t.Followup_FollowupId)
                .ForeignKey("dbo.User", t => t.User_UserId)
                .Index(t => t.Category_CategoryId)
                .Index(t => t.Followup_FollowupId)
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
                "dbo.Followup",
                c => new
                    {
                        FollowupId = c.Int(nullable: false, identity: true),
                        Text = c.String(maxLength: 2000),
                        CreatedDateTime = c.DateTime(nullable: false),
                        FollowupState = c.Int(nullable: false),
                        Question_QuestionId = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FollowupId)
                .ForeignKey("dbo.Question", t => t.Question_QuestionId)
                .ForeignKey("dbo.User", t => t.User_UserId)
                .Index(t => t.Question_QuestionId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UserType = c.Int(nullable: false),
                        HistoricPoints = c.Int(nullable: false),
                        HistoricQuestions = c.Int(nullable: false),
                        HistoricAnswers = c.Int(nullable: false),
                        HistoricStarVotes = c.Int(nullable: false),
                        HistoricPopularVotes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Bookmark",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedDateTime = c.DateTime(nullable: false),
                        Question_QuestionId = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Question", t => t.Question_QuestionId)
                .ForeignKey("dbo.User", t => t.User_UserId)
                .Index(t => t.Question_QuestionId)
                .Index(t => t.User_UserId);
            
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
                "dbo.LastRunLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExpireAnswerSlots = c.DateTime(),
                        CloseQuestions = c.DateTime(),
                        RetireOldQuestions = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ModerationEntry",
                c => new
                    {
                        ModerationEntryId = c.Int(nullable: false, identity: true),
                        State = c.Int(nullable: false),
                        Message = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        AddedByUser_UserId = c.Int(nullable: false),
                        Moderation_ModerationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ModerationEntryId)
                .ForeignKey("dbo.User", t => t.AddedByUser_UserId)
                .ForeignKey("dbo.Moderation", t => t.Moderation_ModerationId)
                .Index(t => t.AddedByUser_UserId)
                .Index(t => t.Moderation_ModerationId);
            
            CreateTable(
                "dbo.Moderation",
                c => new
                    {
                        ModerationId = c.Int(nullable: false, identity: true),
                        ModerationFor = c.Int(nullable: false),
                        Answer_AnswerId = c.Int(),
                        Followup_FollowupId = c.Int(),
                        ForUser_UserId = c.Int(nullable: false),
                        Question_QuestionId = c.Int(),
                    })
                .PrimaryKey(t => t.ModerationId)
                .ForeignKey("dbo.Answer", t => t.Answer_AnswerId)
                .ForeignKey("dbo.Followup", t => t.Followup_FollowupId)
                .ForeignKey("dbo.User", t => t.ForUser_UserId)
                .ForeignKey("dbo.Question", t => t.Question_QuestionId)
                .Index(t => t.Answer_AnswerId)
                .Index(t => t.Followup_FollowupId)
                .Index(t => t.ForUser_UserId)
                .Index(t => t.Question_QuestionId);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        NotificationType = c.Int(nullable: false),
                        NotificationTarget = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        ActionedDateTime = c.DateTime(),
                        Answer_AnswerId = c.Int(),
                        Followup_FollowupId = c.Int(),
                        ForUser_UserId = c.Int(nullable: false),
                        Question_QuestionId = c.Int(),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.Answer", t => t.Answer_AnswerId)
                .ForeignKey("dbo.Followup", t => t.Followup_FollowupId)
                .ForeignKey("dbo.User", t => t.ForUser_UserId)
                .ForeignKey("dbo.Question", t => t.Question_QuestionId)
                .Index(t => t.Answer_AnswerId)
                .Index(t => t.Followup_FollowupId)
                .Index(t => t.ForUser_UserId)
                .Index(t => t.Question_QuestionId);
            
            CreateTable(
                "dbo.PointConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PointType = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Points = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rank",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PointsRequired = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReportedPost",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reason = c.Int(nullable: false),
                        ReportedDateTime = c.DateTime(nullable: false),
                        Answer_AnswerId = c.Int(),
                        ByUser_UserId = c.Int(nullable: false),
                        Question_QuestionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answer", t => t.Answer_AnswerId)
                .ForeignKey("dbo.User", t => t.ByUser_UserId)
                .ForeignKey("dbo.Question", t => t.Question_QuestionId)
                .Index(t => t.Answer_AnswerId)
                .Index(t => t.ByUser_UserId)
                .Index(t => t.Question_QuestionId);
            
            CreateTable(
                "dbo.ServerConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ServerRole = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SystemConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaxAnswers = c.Int(nullable: false),
                        QuestionLifetime = c.Int(nullable: false),
                        SystemEnvironment = c.Int(nullable: false),
                        RetireQuestionAfterDays = c.Int(nullable: false),
                        ExpireAnswerSlotsAfterMinutes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserPoint",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PointType = c.Int(nullable: false),
                        PointsAwarded = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        ForUser_UserId = c.Int(nullable: false),
                        RelatedQuestion_QuestionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.ForUser_UserId)
                .ForeignKey("dbo.Question", t => t.RelatedQuestion_QuestionId)
                .Index(t => t.ForUser_UserId)
                .Index(t => t.RelatedQuestion_QuestionId);
            
            CreateTable(
                "dbo.Vote",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedDateTime = c.DateTime(nullable: false),
                        Answer_AnswerId = c.Int(nullable: false),
                        Question_QuestionId = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answer", t => t.Answer_AnswerId)
                .ForeignKey("dbo.Question", t => t.Question_QuestionId)
                .ForeignKey("dbo.User", t => t.User_UserId)
                .Index(t => t.Answer_AnswerId)
                .Index(t => t.Question_QuestionId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.QuestionRetirement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ModerationRetirement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModerationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserPointRetirement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TotalPoints = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoteCountRetirement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        AnswerId = c.Int(nullable: false),
                        Votes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vote", "User_UserId", "dbo.User");
            DropForeignKey("dbo.Vote", "Question_QuestionId", "dbo.Question");
            DropForeignKey("dbo.Vote", "Answer_AnswerId", "dbo.Answer");
            DropForeignKey("dbo.UserPoint", "RelatedQuestion_QuestionId", "dbo.Question");
            DropForeignKey("dbo.UserPoint", "ForUser_UserId", "dbo.User");
            DropForeignKey("dbo.ReportedPost", "Question_QuestionId", "dbo.Question");
            DropForeignKey("dbo.ReportedPost", "ByUser_UserId", "dbo.User");
            DropForeignKey("dbo.ReportedPost", "Answer_AnswerId", "dbo.Answer");
            DropForeignKey("dbo.Notification", "Question_QuestionId", "dbo.Question");
            DropForeignKey("dbo.Notification", "ForUser_UserId", "dbo.User");
            DropForeignKey("dbo.Notification", "Followup_FollowupId", "dbo.Followup");
            DropForeignKey("dbo.Notification", "Answer_AnswerId", "dbo.Answer");
            DropForeignKey("dbo.Moderation", "Question_QuestionId", "dbo.Question");
            DropForeignKey("dbo.ModerationEntry", "Moderation_ModerationId", "dbo.Moderation");
            DropForeignKey("dbo.Moderation", "ForUser_UserId", "dbo.User");
            DropForeignKey("dbo.Moderation", "Followup_FollowupId", "dbo.Followup");
            DropForeignKey("dbo.Moderation", "Answer_AnswerId", "dbo.Answer");
            DropForeignKey("dbo.ModerationEntry", "AddedByUser_UserId", "dbo.User");
            DropForeignKey("dbo.Bookmark", "User_UserId", "dbo.User");
            DropForeignKey("dbo.Bookmark", "Question_QuestionId", "dbo.Question");
            DropForeignKey("dbo.Answer", "User_UserId", "dbo.User");
            DropForeignKey("dbo.Question", "User_UserId", "dbo.User");
            DropForeignKey("dbo.Question", "Followup_FollowupId", "dbo.Followup");
            DropForeignKey("dbo.Followup", "User_UserId", "dbo.User");
            DropForeignKey("dbo.Followup", "Question_QuestionId", "dbo.Question");
            DropForeignKey("dbo.Question", "Category_CategoryId", "dbo.Category");
            DropForeignKey("dbo.Answer", "Question_QuestionId", "dbo.Question");
            DropIndex("dbo.Vote", new[] { "User_UserId" });
            DropIndex("dbo.Vote", new[] { "Question_QuestionId" });
            DropIndex("dbo.Vote", new[] { "Answer_AnswerId" });
            DropIndex("dbo.UserPoint", new[] { "RelatedQuestion_QuestionId" });
            DropIndex("dbo.UserPoint", new[] { "ForUser_UserId" });
            DropIndex("dbo.ReportedPost", new[] { "Question_QuestionId" });
            DropIndex("dbo.ReportedPost", new[] { "ByUser_UserId" });
            DropIndex("dbo.ReportedPost", new[] { "Answer_AnswerId" });
            DropIndex("dbo.Notification", new[] { "Question_QuestionId" });
            DropIndex("dbo.Notification", new[] { "ForUser_UserId" });
            DropIndex("dbo.Notification", new[] { "Followup_FollowupId" });
            DropIndex("dbo.Notification", new[] { "Answer_AnswerId" });
            DropIndex("dbo.Moderation", new[] { "Question_QuestionId" });
            DropIndex("dbo.Moderation", new[] { "ForUser_UserId" });
            DropIndex("dbo.Moderation", new[] { "Followup_FollowupId" });
            DropIndex("dbo.Moderation", new[] { "Answer_AnswerId" });
            DropIndex("dbo.ModerationEntry", new[] { "Moderation_ModerationId" });
            DropIndex("dbo.ModerationEntry", new[] { "AddedByUser_UserId" });
            DropIndex("dbo.Bookmark", new[] { "User_UserId" });
            DropIndex("dbo.Bookmark", new[] { "Question_QuestionId" });
            DropIndex("dbo.Followup", new[] { "User_UserId" });
            DropIndex("dbo.Followup", new[] { "Question_QuestionId" });
            DropIndex("dbo.Question", new[] { "User_UserId" });
            DropIndex("dbo.Question", new[] { "Followup_FollowupId" });
            DropIndex("dbo.Question", new[] { "Category_CategoryId" });
            DropIndex("dbo.Answer", new[] { "User_UserId" });
            DropIndex("dbo.Answer", new[] { "Question_QuestionId" });
            DropTable("dbo.VoteCountRetirement");
            DropTable("dbo.UserPointRetirement");
            DropTable("dbo.ModerationRetirement");
            DropTable("dbo.QuestionRetirement");
            DropTable("dbo.Vote");
            DropTable("dbo.UserPoint");
            DropTable("dbo.SystemConfiguration");
            DropTable("dbo.ServerConfiguration");
            DropTable("dbo.ReportedPost");
            DropTable("dbo.Rank");
            DropTable("dbo.PointConfiguration");
            DropTable("dbo.Notification");
            DropTable("dbo.Moderation");
            DropTable("dbo.ModerationEntry");
            DropTable("dbo.LastRunLog");
            DropTable("dbo.DevelopmentUser");
            DropTable("dbo.Bookmark");
            DropTable("dbo.User");
            DropTable("dbo.Followup");
            DropTable("dbo.Category");
            DropTable("dbo.Question");
            DropTable("dbo.Answer");
        }
    }
}
