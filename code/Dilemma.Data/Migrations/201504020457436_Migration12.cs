namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration12 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.User", "HistoricBestAnswers", "HistoricStarVotes");
            RenameColumn("dbo.User", "HistoricFavouriteAnswers", "HistoricPopularVotes");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.User", "HistoricStarVotes", "HistoricBestAnswers");
            RenameColumn("dbo.User", "HistoricPopularVotes", "HistoricFavouriteAnswers");
        }
    }
}
