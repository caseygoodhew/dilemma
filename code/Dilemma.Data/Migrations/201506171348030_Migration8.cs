namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration8 : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE [SystemConfiguration] SET QuestionLifetime = 21");
        }
        
        public override void Down()
        {
        }
    }
}
