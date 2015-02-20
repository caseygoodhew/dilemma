namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemConfiguration", "ExpireAnswerSlotsAfterMinutes", c => c.Int(nullable: false, defaultValue: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemConfiguration", "ExpireAnswerSlotsAfterMinutes");
        }
    }
}
