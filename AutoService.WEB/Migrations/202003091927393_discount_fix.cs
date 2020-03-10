namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class discount_fix : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Discounts", "isActive");
        }

        public override void Down()
        {
            AddColumn("dbo.Discounts", "isActive", c => c.Boolean(nullable: false));
        }
    }
}