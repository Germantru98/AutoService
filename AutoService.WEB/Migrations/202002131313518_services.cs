namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class services : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceImages",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Image = c.Binary(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Services",
                c => new
                {
                    ServiceId = c.Int(nullable: false, identity: true),
                    ServiceName = c.String(nullable: false),
                    ServiceImageId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ServiceId)
                .ForeignKey("dbo.ServiceImages", t => t.ServiceImageId, cascadeDelete: true)
                .Index(t => t.ServiceImageId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Services", "ServiceImageId", "dbo.ServiceImages");
            DropIndex("dbo.Services", new[] { "ServiceImageId" });
            DropTable("dbo.Services");
            DropTable("dbo.ServiceImages");
        }
    }
}