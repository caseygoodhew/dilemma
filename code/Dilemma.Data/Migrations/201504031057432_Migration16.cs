namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration16 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRank",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PointsRequired = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserRank");
        }
    }
}
