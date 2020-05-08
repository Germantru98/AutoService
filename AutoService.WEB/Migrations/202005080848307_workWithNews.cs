namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class workWithNews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "HomeMainCarouselItemId", c => c.Int());
            CreateIndex("dbo.News", "HomeMainCarouselItemId");
            AddForeignKey("dbo.News", "HomeMainCarouselItemId", "dbo.HomeMainCarouselItems", "Id");
            DropColumn("dbo.HomeMainCarouselItems", "NewsId");
        }

        public override void Down()
        {
            AddColumn("dbo.HomeMainCarouselItems", "NewsId", c => c.Int());
            DropForeignKey("dbo.News", "HomeMainCarouselItemId", "dbo.HomeMainCarouselItems");
            DropIndex("dbo.News", new[] { "HomeMainCarouselItemId" });
            DropColumn("dbo.News", "HomeMainCarouselItemId");
        }
    }
}