namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class services2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Price", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Services", "Price");
        }
    }
}