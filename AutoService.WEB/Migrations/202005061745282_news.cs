namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class news : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HomeMainCarouselItems", "NewsId", c => c.Int());
            DropColumn("dbo.HomeMainCarouselItems", "RouteHref");
        }

        public override void Down()
        {
            AddColumn("dbo.HomeMainCarouselItems", "RouteHref", c => c.String());
            DropColumn("dbo.HomeMainCarouselItems", "NewsId");
        }
    }
}