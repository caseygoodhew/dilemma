namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemConfiguration", "QuestionLifetimeDays", c => c.Int(nullable: false));
            Sql("UPDATE [SystemConfiguration] SET QuestionLifetimeDays = QuestionLifetime");
            DropColumn("dbo.SystemConfiguration", "QuestionLifetime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SystemConfiguration", "QuestionLifetime", c => c.Int(nullable: false));
            Sql("UPDATE [SystemConfiguration] SET QuestionLifetime = QuestionLifetimeDays");
            DropColumn("dbo.SystemConfiguration", "QuestionLifetimeDays");
        }
    }
}
