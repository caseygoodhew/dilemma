#region Component Designer generated code
namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answer", "Question_QuestionId", "dbo.Question");
            DropIndex("dbo.Answer", new[] { "Question_QuestionId" });
            AlterColumn("dbo.Answer", "Question_QuestionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Answer", "Question_QuestionId");
            AddForeignKey("dbo.Answer", "Question_QuestionId", "dbo.Question", "QuestionId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answer", "Question_QuestionId", "dbo.Question");
            DropIndex("dbo.Answer", new[] { "Question_QuestionId" });
            AlterColumn("dbo.Answer", "Question_QuestionId", c => c.Int());
            CreateIndex("dbo.Answer", "Question_QuestionId");
            AddForeignKey("dbo.Answer", "Question_QuestionId", "dbo.Question", "QuestionId");
        }
    }
}
#endregion