namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class newShopCart : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.BasketItems");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.BasketItems",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ServiceId = c.Int(nullable: false),
                    UserId = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }
    }
}