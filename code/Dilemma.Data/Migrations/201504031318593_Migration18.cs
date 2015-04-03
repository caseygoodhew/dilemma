namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration18 : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookmark", "User_UserId", "dbo.User");
            DropForeignKey("dbo.Bookmark", "Question_QuestionId", "dbo.Question");
            DropIndex("dbo.Bookmark", new[] { "User_UserId" });
            DropIndex("dbo.Bookmark", new[] { "Question_QuestionId" });
            DropTable("dbo.Bookmark");
        }
    }
}
