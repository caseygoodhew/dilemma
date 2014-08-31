namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answer", "User_UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Question", "User_UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Answer", "User_UserId");
            CreateIndex("dbo.Question", "User_UserId");
            AddForeignKey("dbo.Question", "User_UserId", "dbo.User", "UserId");
            AddForeignKey("dbo.Answer", "User_UserId", "dbo.User", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answer", "User_UserId", "dbo.User");
            DropForeignKey("dbo.Question", "User_UserId", "dbo.User");
            DropIndex("dbo.Question", new[] { "User_UserId" });
            DropIndex("dbo.Answer", new[] { "User_UserId" });
            DropColumn("dbo.Question", "User_UserId");
            DropColumn("dbo.Answer", "User_UserId");
        }
    }
}
