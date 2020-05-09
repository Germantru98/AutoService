namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class deleteMCI : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.HomeMainCarouselItems");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.HomeMainCarouselItems",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    Description = c.String(),
                    ImageHref = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }
    }
}