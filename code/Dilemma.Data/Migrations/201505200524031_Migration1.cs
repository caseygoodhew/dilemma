namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemConfiguration", "EnableWebPurify", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemConfiguration", "EmailErrors", c => c.Boolean(nullable: false));
            AddColumn("dbo.SystemConfiguration", "EmailErrorsTo", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemConfiguration", "EmailErrorsTo");
            DropColumn("dbo.SystemConfiguration", "EmailErrors");
            DropColumn("dbo.SystemConfiguration", "EnableWebPurify");
        }
    }
}
