namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "ClosesDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Question", "MaxAnswers", c => c.Int(nullable: false));
            AddColumn("dbo.SystemConfiguration", "SystemEnvironment", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemConfiguration", "SystemEnvironment");
            DropColumn("dbo.Question", "MaxAnswers");
            DropColumn("dbo.Question", "ClosesDateTime");
        }
    }
}
