namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class service3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Services", "ServiceImageId", "dbo.ServiceImages");
            DropIndex("dbo.Services", new[] { "ServiceImageId" });
            AddColumn("dbo.Services", "Image", c => c.Binary());
            DropColumn("dbo.Services", "ServiceImageId");
            DropTable("dbo.ServiceImages");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.ServiceImages",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Image = c.Binary(),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Services", "ServiceImageId", c => c.Int(nullable: false));
            DropColumn("dbo.Services", "Image");
            CreateIndex("dbo.Services", "ServiceImageId");
            AddForeignKey("dbo.Services", "ServiceImageId", "dbo.ServiceImages", "Id", cascadeDelete: true);
        }
    }
}