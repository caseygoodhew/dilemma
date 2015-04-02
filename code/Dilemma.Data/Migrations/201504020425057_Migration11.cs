namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "HistoricAnswers", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.User", "HistoricBestAnswers", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.User", "HistoricFavouriteAnswers", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "HistoricFavouriteAnswers");
            DropColumn("dbo.User", "HistoricBestAnswers");
            DropColumn("dbo.User", "HistoricAnswers");
        }
    }
}
