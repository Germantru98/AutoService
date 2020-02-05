namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class carupd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cars", "Year", c => c.String());
        }

        public override void Down()
        {
            AlterColumn("dbo.Cars", "Year", c => c.DateTime(nullable: false));
        }
    }
}