namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration10 : DbMigration
    {
        public override void Up()
        {
            Sql("update rank set pointsrequired = 0 where name = 'Teacher'");
            Sql("update rank set pointsrequired = 1000 where name = 'Wise Owl'");
            Sql("update rank set pointsrequired = 5000 where name = 'Guru'");
            Sql("update rank set pointsrequired = 15000 where name = 'Angel'");
            Sql("update rank set pointsrequired = 50000 where name = 'Deity'");
        }
        
        public override void Down()
        {
            Sql("update rank set pointsrequired = 0 where name = 'Teacher'");
            Sql("update rank set pointsrequired = 100 where name = 'Wise Owl'");
            Sql("update rank set pointsrequired = 1000 where name = 'Guru'");
            Sql("update rank set pointsrequired = 5000 where name = 'Angel'");
            Sql("update rank set pointsrequired = 20000 where name = 'Deity'");
        }
    }
}
