namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answer", "LastTouchedDateTime", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            Sql("UPDATE [dbo].[Answer] SET LastTouchedDateTime = CreatedDateTime");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answer", "LastTouchedDateTime");
        }
    }
}
