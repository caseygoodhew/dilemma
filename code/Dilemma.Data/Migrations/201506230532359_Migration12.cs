namespace Dilemma.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration12 : DbMigration
    {
        public override void Up()
        {
            Sql("update rank set name = 'Scholar' where name = 'Teacher'");
            Sql("update rank set name = 'Guardian' where name = 'Angel'");
        }
        
        public override void Down()
        {
        }
    }
}
