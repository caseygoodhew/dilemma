namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Question", "ClosedDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Question", "ClosedDateTime", c => c.DateTime(nullable: false));
        }
    }
}
