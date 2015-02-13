namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration1 : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vote", "User_UserId", "dbo.User");
            DropForeignKey("dbo.Vote", "Question_QuestionId", "dbo.Question");
            DropForeignKey("dbo.Vote", "Answer_AnswerId", "dbo.Answer");
            DropIndex("dbo.Vote", new[] { "User_UserId" });
            DropIndex("dbo.Vote", new[] { "Question_QuestionId" });
            DropIndex("dbo.Vote", new[] { "Answer_AnswerId" });
            DropTable("dbo.Vote");
        }
    }
}
