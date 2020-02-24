namespace AutoService.WEB.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class basket_start : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BasketItems",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ServiceId = c.Int(nullable: false),
                    UserId = c.String(),
                    ApplicationUser_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.BasketItems", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.BasketItems", new[] { "ApplicationUser_Id" });
            DropTable("dbo.BasketItems");
        }
    }
}