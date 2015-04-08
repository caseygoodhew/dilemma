namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration20 : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportedPost", "Question_QuestionId", "dbo.Question");
            DropForeignKey("dbo.ReportedPost", "ByUser_UserId", "dbo.User");
            DropForeignKey("dbo.ReportedPost", "Answer_AnswerId", "dbo.Answer");
            DropIndex("dbo.ReportedPost", new[] { "Question_QuestionId" });
            DropIndex("dbo.ReportedPost", new[] { "ByUser_UserId" });
            DropIndex("dbo.ReportedPost", new[] { "Answer_AnswerId" });
            DropTable("dbo.ReportedPost");
        }
    }
}
