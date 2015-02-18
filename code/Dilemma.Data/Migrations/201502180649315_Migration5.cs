namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemConfiguration", "RetireQuestionAfterDays", c => c.Int(nullable: false, defaultValue: 14));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemConfiguration", "RetireQuestionAfterDays");
        }
    }
}
