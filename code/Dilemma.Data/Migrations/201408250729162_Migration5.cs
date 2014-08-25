namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answer", "AnswerType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answer", "AnswerType");
        }
    }
}
