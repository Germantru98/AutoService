namespace AutoService.WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _124 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HomeMainCarouselItems",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    Description = c.String(),
                    ImageHref = c.String(),
                    RouteHref = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.HomeMainCarouselItems");
        }
    }
}
