namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PointConfiguration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PointType = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Points = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PointConfiguration");
        }
    }
}
