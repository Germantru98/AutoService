namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class discv5 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Discounts", "ServiceNavigationId");
        }

        public override void Down()
        {
            AddColumn("dbo.Discounts", "ServiceNavigationId", c => c.Int(nullable: false));
        }
    }
}