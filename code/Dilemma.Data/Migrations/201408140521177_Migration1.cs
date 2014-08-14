namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemConfiguration", "QuestionLifetime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemConfiguration", "QuestionLifetime");
        }
    }
}
