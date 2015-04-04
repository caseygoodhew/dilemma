namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration19 : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.Question", "Followup_FollowupId", c => c.Int());
            AddColumn("dbo.Moderation", "Followup_FollowupId", c => c.Int());
            CreateIndex("dbo.Question", "Followup_FollowupId");
            CreateIndex("dbo.Moderation", "Followup_FollowupId");
            AddForeignKey("dbo.Question", "Followup_FollowupId", "dbo.Followup", "FollowupId");
            AddForeignKey("dbo.Moderation", "Followup_FollowupId", "dbo.Followup", "FollowupId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Moderation", "Followup_FollowupId", "dbo.Followup");
            DropForeignKey("dbo.Question", "Followup_FollowupId", "dbo.Followup");
            DropForeignKey("dbo.Followup", "User_UserId", "dbo.User");
            DropForeignKey("dbo.Followup", "Question_QuestionId", "dbo.Question");
            DropIndex("dbo.Moderation", new[] { "Followup_FollowupId" });
            DropIndex("dbo.Followup", new[] { "User_UserId" });
            DropIndex("dbo.Followup", new[] { "Question_QuestionId" });
            DropIndex("dbo.Question", new[] { "Followup_FollowupId" });
            DropColumn("dbo.Moderation", "Followup_FollowupId");
            DropColumn("dbo.Question", "Followup_FollowupId");
            DropTable("dbo.Followup");
        }
    }
}
