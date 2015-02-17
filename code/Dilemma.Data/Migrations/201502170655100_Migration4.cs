namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration4 : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserPointRetirement");
            DropTable("dbo.ModerationRetirement");
            DropTable("dbo.QuestionRetirement");
        }
    }
}
