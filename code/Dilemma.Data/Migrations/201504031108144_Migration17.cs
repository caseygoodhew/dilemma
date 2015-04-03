namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration17 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserRank", newName: "Rank");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Rank", newName: "UserRank");
        }
    }
}
