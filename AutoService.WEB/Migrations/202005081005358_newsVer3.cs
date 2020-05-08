namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class newsVer3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.News", "HomeMainCarouselItemId", "dbo.HomeMainCarouselItems");
            DropIndex("dbo.News", new[] { "HomeMainCarouselItemId" });
            AddColumn("dbo.News", "SlideTitle", c => c.String());
            AddColumn("dbo.News", "SlideDescription", c => c.String());
            AddColumn("dbo.News", "ImgHref", c => c.String());
            DropColumn("dbo.News", "HomeMainCarouselItemId");
        }

        public override void Down()
        {
            AddColumn("dbo.News", "HomeMainCarouselItemId", c => c.Int());
            DropColumn("dbo.News", "ImgHref");
            DropColumn("dbo.News", "SlideDescription");
            DropColumn("dbo.News", "SlideTitle");
            CreateIndex("dbo.News", "HomeMainCarouselItemId");
            AddForeignKey("dbo.News", "HomeMainCarouselItemId", "dbo.HomeMainCarouselItems", "Id");
        }
    }
}