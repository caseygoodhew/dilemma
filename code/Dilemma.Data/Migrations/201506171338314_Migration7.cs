namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration7 : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE [SystemConfiguration] DROP CONSTRAINT [FK_SystemConfiguration_QuestionLifetime]");
            Sql("DROP TABLE [Enum_QuestionLifetime]");
        }
        
        public override void Down()
        {
        }
    }
}
