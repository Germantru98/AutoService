namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class cbrands : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarBrands",
                c => new
                {
                    BrandId = c.Int(nullable: false, identity: true),
                    BrandName = c.String(),
                    ImageHref = c.String(),
                })
                .PrimaryKey(t => t.BrandId);
        }

        public override void Down()
        {
            DropTable("dbo.CarBrands");
        }
    }
}