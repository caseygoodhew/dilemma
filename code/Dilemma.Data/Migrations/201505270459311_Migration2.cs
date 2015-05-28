namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailLogLevel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogLevel = c.String(nullable: false),
                        EnableEmails = c.Boolean(nullable: false),
                        SendToEmailAddresses = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailLogLevel");
        }
    }
}
