namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SystemConfiguration", "EmailErrorsTo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SystemConfiguration", "EmailErrorsTo", c => c.String(nullable: false));
        }
    }
}
