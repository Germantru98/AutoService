namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Adddatetorewiew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserRewiews", "Date", c => c.DateTime(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.UserRewiews", "Date");
        }
    }
}