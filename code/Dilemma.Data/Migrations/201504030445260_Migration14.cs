namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "HistoricQuestions", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "HistoricQuestions");
        }
    }
}
