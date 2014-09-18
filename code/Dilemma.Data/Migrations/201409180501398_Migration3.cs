namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Question_QuestionId", c => c.Int());
            CreateIndex("dbo.Notification", "Question_QuestionId");
            AddForeignKey("dbo.Notification", "Question_QuestionId", "dbo.Question", "QuestionId");
            DropColumn("dbo.Notification", "NotificationTypeKey");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notification", "NotificationTypeKey", c => c.String(nullable: false));
            DropForeignKey("dbo.Notification", "Question_QuestionId", "dbo.Question");
            DropIndex("dbo.Notification", new[] { "Question_QuestionId" });
            DropColumn("dbo.Notification", "Question_QuestionId");
        }
    }
}
