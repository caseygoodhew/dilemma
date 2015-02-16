namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answer", "Vote_Id", "dbo.Vote");
            DropIndex("dbo.Answer", new[] { "Vote_Id" });
            DropColumn("dbo.Answer", "Vote_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Answer", "Vote_Id", c => c.Int());
            CreateIndex("dbo.Answer", "Vote_Id");
            AddForeignKey("dbo.Answer", "Vote_Id", "dbo.Vote", "Id");
        }
    }
}
