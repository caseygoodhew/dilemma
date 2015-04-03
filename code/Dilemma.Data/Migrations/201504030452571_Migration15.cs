namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration15 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.VoteCount", newName: "VoteCountRetirement");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.VoteCountRetirement", newName: "VoteCount");
        }
    }
}
