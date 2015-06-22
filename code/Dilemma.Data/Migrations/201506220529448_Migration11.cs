namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration11 : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE [dbo].[Question] SET ClosesDateTime = DATEADD(DAY, 21, CreatedDateTime) WHERE ClosesDateTime > DATEADD(DAY, 21, CreatedDateTime) AND ClosedDateTime is NULL");
        }
        
        public override void Down()
        {
        }
    }
}
