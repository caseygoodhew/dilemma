namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration10 : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LastRunLog");
        }
    }
}
